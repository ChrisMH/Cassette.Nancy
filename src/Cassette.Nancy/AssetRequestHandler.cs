using System;
using System.Linq;
using Cassette.Utilities;
using NLog;
using Nancy;
using Nancy.Responses;

namespace Cassette.Nancy
{
  class AssetRequestHandler : ICassetteRequestHandler
  {
    public const string PathPrefix = "/asset";
    private readonly BundleCollection bundles;
    private readonly Logger logger;

    public AssetRequestHandler(BundleCollection bundles)
    {
      this.bundles = bundles;
      this.logger = NLog.LogManager.GetCurrentClassLogger();
    }

    public Response ProcessRequest(NancyContext context)
    {
      var path = string.Concat("~", context.Request.Path.Substring(PathPrefix.Length));

      using (bundles.GetReadLock())
      {
        Bundle bundle;
        IAsset asset;
        if (!bundles.TryGetAssetByPath(path, out asset, out bundle))
        {
          logger.Info("ProcessRequest : asset bundle '{0}' was not found", path);
          return new HtmlResponse(HttpStatusCode.NotFound);
        }

        var actualETag = "\"" + asset.Hash.ToHexString() + "\"";
        var givenETag = context.Request.Headers["If-None-Match"];

        if(givenETag.Equals(actualETag))
        {
          logger.Info("ProcessRequest : asset bundle '{0}' was not modified", path);
          var notModified = new HtmlResponse(HttpStatusCode.NotModified);
          notModified.ContentType = bundle.ContentType;
          return notModified;
        }

        logger.Info("ProcessRequest : asset bundle '{0}' found and returned", path);
        var response = new StreamResponse(asset.OpenStream, bundle.ContentType);
        response.WithHeader("ETag", actualETag);
        return response;
      }
    }

  }
}