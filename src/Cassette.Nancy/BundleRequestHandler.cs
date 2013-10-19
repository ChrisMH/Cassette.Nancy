using System;
using System.Linq;
using System.Text.RegularExpressions;
using Cassette.Utilities;
using Nancy;
using Nancy.Responses;

namespace Cassette.Nancy
{
  internal class BundleRequestHandler<TBundle> : ICassetteRequestHandler
    where TBundle : Bundle
  {
    
    private readonly BundleCollection bundles;
    private readonly NLog.Logger logger;

    public BundleRequestHandler(BundleCollection bundles)
    {
      this.bundles = bundles;
      this.logger = NLog.LogManager.GetCurrentClassLogger();
    }

    public Response ProcessRequest(NancyContext context, string path)
    {
      var pattern = new Regex(@"~?/?[a-z]{5,}/[a-f0-9]{10,}/", RegexOptions.IgnoreCase);
      path = pattern.Replace(path, "/");

      using (bundles.GetReadLock())
      {
        var bundle = bundles.FindBundlesContainingPath(path).OfType<TBundle>().FirstOrDefault();
        if (bundle == null)
        {
          logger.Info("ProcessRequest : Bundle '{0}' not found", path);
          return new HtmlResponse(HttpStatusCode.NotFound);
        }

        var actualETag = "\"" + bundle.Hash.ToHexString() + "\"";
        var givenETag = context.Request.Headers["If-None-Match"];

        if(givenETag.Equals(actualETag))
        {
          logger.Info("ProcessRequest : Bundle '{0}' not modified", path);
          var notModified = new HtmlResponse(HttpStatusCode.NotModified);
          notModified.ContentType = bundle.ContentType;
          return notModified;
        }

        logger.Info("ProcessRequest : Bundle '{0}' returned", path);
        var response = new StreamResponse(bundle.OpenStream, bundle.ContentType);
        response.WithHeader("ETag", actualETag);
        return response;
      }
    }
    
    public static string PathPrefix
    {
      get
      {
        if(typeof(TBundle).Equals(typeof(Scripts.ScriptBundle)))
          return "/script";
        else if(typeof(TBundle).Equals(typeof(Stylesheets.StylesheetBundle)))
          return "/stylesheet";
        else if(typeof(TBundle).Equals(typeof(HtmlTemplates.HtmlTemplateBundle)))
          return "/htmltemplate";
        else
          throw new ApplicationException("Unhandled bundle type");
      }
    }
  }
}