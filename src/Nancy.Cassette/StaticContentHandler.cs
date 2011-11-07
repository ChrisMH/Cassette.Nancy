using System;
using Cassette;
using Nancy.Responses;
using Utility.Logging;

namespace Nancy.Cassette
{
  namespace Nancy.Cassette
  {
    public class StaticContentHandler : ICassetteHandler
    {
      public StaticContentHandler(string handlerRoot, Func<string, Module> findModuleForPath, ILogger logger)
      {
        this.handlerRoot = handlerRoot;
        this.findModuleForPath = findModuleForPath;
        if (logger != null) this.logger = logger.GetCurrentClassLogger();
      }

      public Response ProcessRequest(NancyContext context)
      {
        if (!context.Request.Url.Path.StartsWith(handlerRoot, StringComparison.InvariantCultureIgnoreCase))
        {
          return null;
        }

        string path, query;
        UrlAndPathGenerator.RemoveUrlQuery(string.Concat("~", context.Request.Url.Path), out path, out query);
        
        var module = findModuleForPath(path);
        if (module == null)
        {
          if (logger != null) logger.Error("StaticContentHandler.ProcessRequest : Module not found for path '{0}'", context.Request.Url.Path);
          return null;
        }

        var asset = module.FindAssetByPath(path);
        if (asset == null)
        {
          if (logger != null) logger.Error("StaticContentHandler.ProcessRequest : Asset not found '{0}'", context.Request.Url.Path);
          return null;
        }

        var response = new StreamResponse(asset.OpenStream, module.ContentType);
        if (logger != null) logger.Trace("StaticContentHandler.ProcessRequest : Returned response for '{0}'", context.Request.Url.Path);
        return response;
      }

      private readonly string handlerRoot;
      private readonly Func<string, Module> findModuleForPath;
      private readonly ILogger logger;
    }
  }
}