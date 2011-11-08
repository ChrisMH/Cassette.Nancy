using System;
using Cassette;
using Nancy.Responses;
using Utility.Logging;

namespace Nancy.Cassette
{
  internal class BundleRouteHandler : CassetteRouteHandlerBase
  {
    public BundleRouteHandler(IBundleContainer bundleContainer, string handlerRoot, ILogger logger)
      : base(bundleContainer, handlerRoot, logger)
    {
    }

    public override Response ProcessRequest(NancyContext context)
    {
      if (!context.Request.Url.Path.StartsWith(HandlerRoot, StringComparison.InvariantCultureIgnoreCase))
      {
        return null;
      }

      var path = RemoveTrailingHashFromPath(string.Concat("~", context.Request.Url.Path.Remove(0, HandlerRoot.Length)));

      var bundle = BundleContainer.FindBundleContainingPath(path);
      if (bundle == null)
      {
        if (Logger != null) Logger.Error("BundleRouteHandler.ProcessRequest : Bundle not found for path '{0}'", context.Request.Url.Path);
        return null;
      }

      /* TODO : Caching
      var actualETag = string.Concat( "\"", module.Assets[0].Hash.ToHexString(), "\"");
      var givenETag = context.Request.Headers["If-None-Match"];
      if (givenETag == actualETag)
      {
        SendNotModified(actualETag);
      }

      CacheLongTime(actualETag);

      var encoding = request.Headers["Accept-Encoding"];
      response.Filter = EncodeStreamAndAppendResponseHeaders(response.Filter, encoding);
      */

      var response = new StreamResponse(() => bundle.Assets[0].OpenStream(), bundle.ContentType);
      if (Logger != null) Logger.Trace("BundleRouteHandler.ProcessRequest : Returned response for '{0}'", context.Request.Url.Path);
      return response;
    }


    /// <summary>
    /// A Module URL has the hash appended after an underscore character. This method removes the underscore and hash from the path.
    /// </summary>
    private string RemoveTrailingHashFromPath(string path)
    {
      var index = path.LastIndexOf('_');
      if (index >= 0)
      {
        return path.Substring(0, index);
      }
      return path;
    }
  }
}