using System;
using System.Linq;
using System.Text.RegularExpressions;
using Nancy;
using Nancy.Responses;

namespace Cassette.Nancy
{
  internal class BundleRouteHandler<T> : CassetteRouteHandlerBase
    where T : Bundle
  {
    public BundleRouteHandler(IBundleContainer bundleContainer, string handlerRoot)
      : base(bundleContainer, handlerRoot)
    {
    }

    public override Response ProcessRequest(NancyContext context)
    {
      if (!context.Request.Url.Path.StartsWith(HandlerRoot, StringComparison.InvariantCultureIgnoreCase))
      {
        return null;
      }

      var path = Regex.Replace(string.Concat("~", context.Request.Url.Path.Remove(0, HandlerRoot.Length)), @"_[^_]+$", "");

      var bundles = BundleContainer.FindBundlesContainingPath(path).ToList();
      if (bundles == null || bundles.Count != 1)
      {
        //if (Logger != null) Logger.Error("BundleRouteHandler.ProcessRequest : Bundle not found for path '{0}'", context.Request.Url.Path);
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

      var response = new StreamResponse(() => bundles[0].Assets[0].OpenStream(), bundles[0].ContentType);
      //if (Logger != null) Logger.Trace("BundleRouteHandler.ProcessRequest : Returned response for '{0}'", context.Request.Url.Path);
      return response;
    }
  }
}