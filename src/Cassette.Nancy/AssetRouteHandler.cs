using System;
using System.Linq;
using System.Text.RegularExpressions;
using Nancy;
using Nancy.Responses;
using Utility.Logging;

namespace Cassette.Nancy
{
  internal class AssetRouteHandler : CassetteRouteHandlerBase
  {
    public AssetRouteHandler(IBundleContainer bundleContainer, string handlerRoot, ILogger logger)
      : base(bundleContainer, handlerRoot, logger)
    {
    }


    public override Response ProcessRequest(NancyContext context)
    {
      if (!context.Request.Url.Path.StartsWith(HandlerRoot, StringComparison.InvariantCultureIgnoreCase))
      {
        return null;
      }

      var path = Regex.Match(string.Concat("~", context.Request.Url.Path.Remove(0, HandlerRoot.Length)), @"^[^\?]*").Value;

      var bundles = BundleContainer.FindBundlesContainingPath(path).ToList();
      if(bundles == null || bundles.Count != 1)
      {
        if (Logger != null) Logger.Error("AssetRouteHandler.ProcessRequest : Asset not found for '{0}'", context.Request.Url.Path);
        return null;
      }

      //IAsset asset;
      //Bundle bundle;
      //if (!BundleContainer.  TryGetAssetByPath(path, out asset, out bundle))
      //{
      //}

      var response = new StreamResponse(() => bundles[0].Assets[0].OpenStream(), bundles[0].ContentType);//  asset.OpenStream, bundle.ContentType);
      if (Logger != null) Logger.Trace("AssetRouteHandler.ProcessRequest : Returned response for '{0}'", context.Request.Url.Path);
      return response;
    }
  }
}