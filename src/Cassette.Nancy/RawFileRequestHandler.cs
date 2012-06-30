using System;
using System.IO;
using System.Text.RegularExpressions;
using Nancy;
using Nancy.Responses;

namespace Cassette.Nancy
{
  class RawFileRequestHandler : ICassetteRequestHandler
  {
    public const string PathPrefix = "/file";

    private readonly BundleCollection bundles;
    private readonly IRootPathProvider rootPathProvider;
    private readonly NLog.Logger logger;

    public RawFileRequestHandler(BundleCollection bundles, IRootPathProvider rootPathProvider)
    {
      this.bundles = bundles;
      this.rootPathProvider = rootPathProvider;
      this.logger = NLog.LogManager.GetCurrentClassLogger();
    }

    public Response ProcessRequest(NancyContext context, string path)
    {
      path = path.Substring(PathPrefix.Length);

      var match = Regex.Match(path, @"^(?<filename>.*)-[a-z0-9]+\.(?<extension>[a-z]+)$", RegexOptions.IgnoreCase);
      if (match.Success == false)
      {
        logger.Error("ProcessRequest : Invalid file path in URL '{0}'", path);
        return new HtmlResponse(HttpStatusCode.InternalServerError);
      }
      var extension = match.Groups["extension"].Value;

      var filePath = string.Concat(rootPathProvider.GetRootPath(), @"\", match.Groups["filename"].Value.Replace('/', '\\'), ".", match.Groups["extension"].Value);
      filePath = Regex.Replace(filePath, @"\\{2,}", @"\");

      if (!File.Exists(filePath))
      {
        logger.Warn("ProcessRequest : File does not exist '{0}'", filePath);
        return new HtmlResponse(HttpStatusCode.NotFound);
      }
      
      logger.Info("ProcessRequest : File '{0}' returned", path);

      return new StreamResponse(() => File.OpenRead(filePath), MimeTypes.GetMimeType(filePath));
    }

  }
}