using System;
using System.Text.RegularExpressions;
using Cassette;
using Nancy.Responses;
using Utility.Logging;

namespace Nancy.Cassette
{
  internal class AssetRouteHandler : CassetteRouteHandlerBase
  {
    public AssetRouteHandler(IBundleContainer bundleContainer, string HandlerRoot, ILogger Logger)
      : base(bundleContainer, HandlerRoot, Logger)
    {
    }


    public override Response ProcessRequest(NancyContext context)
    {
      if (!context.Request.Url.Path.StartsWith(HandlerRoot, StringComparison.InvariantCultureIgnoreCase))
      {
        return null;
      }

      var path = Regex.Match(string.Concat("~", context.Request.Url.Path.Remove(0, HandlerRoot.Length)), @"^[^\?]*").Value;

      IAsset asset;
      Bundle bundle;
      if (!BundleContainer.TryGetAssetByPath(path, out asset, out bundle))
      {
        if (Logger != null) Logger.Error("AssetRouteHandler.ProcessRequest : Asset not found for '{0}'", context.Request.Url.Path);
        return null;
      }

      var response = new StreamResponse(asset.OpenStream, bundle.ContentType);
      if (Logger != null) Logger.Trace("AssetRouteHandler.ProcessRequest : Returned response for '{0}'", context.Request.Url.Path);
      return response;
    }
  }
}