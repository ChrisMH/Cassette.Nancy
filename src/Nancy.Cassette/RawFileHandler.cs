using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Cassette.IO;
using Nancy.Responses;
using Utility.Logging;

namespace Nancy.Cassette
{
  public class RawFileHandler : ICassetteHandler
  {
    public RawFileHandler(string handlerRoot, string applicationRoot, ILogger logger)
    {
      this.handlerRoot = handlerRoot;
      this.applicationRoot = applicationRoot;
      if(logger != null) this.logger = logger.GetCurrentClassLogger();
    }


    public Response ProcessRequest(NancyContext context)
    {
      if (!context.Request.Url.Path.StartsWith(handlerRoot, StringComparison.InvariantCultureIgnoreCase))
      {
        return null;
      }

      var path = context.Request.Url.Path.Remove(0, handlerRoot.Length + 1);
      var match = Regex.Match(path, @"^(?<filename>.*)_[a-z0-9]+_(?<extension>[a-z]+)$", RegexOptions.IgnoreCase);
      if (match.Success == false)
      {
        if (logger != null) logger.Error("RawFileHandler.ProcessRequest : Invalid file path in URL '{0}'", path);
        return null;
      }
      var extension = match.Groups["extension"].Value;

      var filePath = Path.Combine(applicationRoot, string.Concat(match.Groups["filename"].Value, ".", extension).Replace('/', '\\'));
      if(!File.Exists(filePath))
      {
        if (logger != null) logger.Error("RawFileHandler.ProcessRequest : Raw file does not exist '{0}'", filePath);
        return null;
      }

      var response = new StreamResponse(() => File.OpenRead(filePath), MimeTypes.GetMimeType(filePath));
      if (logger != null) logger.Info("RawFileHandler.ProcessRequest : Returned response for '{0}'", context.Request.Url.Path);
      return response;
    }

    private readonly string handlerRoot;
    private readonly string applicationRoot;
    private readonly ILogger logger;
  }
}