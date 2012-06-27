using System;
using System.Linq;
using Cassette.Utilities;
using Nancy;
using Nancy.Responses;

namespace Cassette.Nancy
{
  class AssetRequestHandler : ICassetteRequestHandler
  {
    public const string PathPrefix = "/asset";

    public AssetRequestHandler(BundleCollection bundles)
    {
      this.bundles = bundles;
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
          var notFound = new Response();
          notFound.StatusCode = HttpStatusCode.NotFound;
          return notFound;
        }

        var actualETag = "\"" + asset.Hash.ToHexString() + "\"";
        var givenETag = context.Request.Headers["If-None-Match"];

        if(givenETag.Equals(actualETag))
        {
          var notModified = new global::Nancy.Response();
          notModified.StatusCode = HttpStatusCode.NotModified;
          notModified.ContentType = bundle.ContentType;
          return notModified;
        }

        var response = new StreamResponse(asset.OpenStream, bundle.ContentType);
        response.WithHeader("ETag", actualETag);
        return response;
      }
    }

    private readonly BundleCollection bundles;
  }
}