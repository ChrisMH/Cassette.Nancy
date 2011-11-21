using System;
using System.IO;
using System.Text.RegularExpressions;
using Nancy;
using Nancy.Responses;
using Utility.Logging;

namespace Cassette.Nancy
{
  internal class RawFileRouteHandler : CassetteRouteHandlerBase
  {
    public RawFileRouteHandler(IBundleContainer bundleContainer, string handlerRoot, ILogger logger, string applicationRoot)
      : base(bundleContainer, handlerRoot, logger)
    {
      this.applicationRoot = applicationRoot;
    }

    public override Response ProcessRequest(NancyContext context)
    {
      if (!context.Request.Url.Path.StartsWith(HandlerRoot, StringComparison.InvariantCultureIgnoreCase))
      {
        return null;
      }

      var path = context.Request.Url.Path.Remove(0, HandlerRoot.Length + 1);
      var match = Regex.Match(path, @"^(?<filename>.*)_[a-z0-9]+_(?<extension>[a-z]+)$", RegexOptions.IgnoreCase);
      if (match.Success == false)
      {
        if (Logger != null) Logger.Error("RawFileRouteHandler.ProcessRequest : Invalid file path in URL '{0}'", path);
        return null;
      }
      var extension = match.Groups["extension"].Value;

      var filePath = Path.Combine(applicationRoot, string.Concat(match.Groups["filename"].Value, ".", extension).Replace('/', '\\'));
      if (!File.Exists(filePath))
      {
        if (Logger != null) Logger.Error("RawFileRouteHandler.ProcessRequest : Raw file does not exist '{0}'", filePath);
        return null;
      }
      if (Logger != null) Logger.Debug("RawFileRouteHandler.ProcessRequest : Loading File: '{0}', MIME Type: {1}", filePath, MimeTypes.GetMimeType(filePath));

      var response = new GenericFileResponse(filePath);// new StreamResponse(() => File.OpenRead(filePath), MimeTypes.GetMimeType(filePath));
      if (Logger != null) Logger.Trace("RawFileRouteHandler.ProcessRequest : Returned response for '{0}'", context.Request.Url.Path);
      return response;
    }

    private readonly string applicationRoot;
  }
}