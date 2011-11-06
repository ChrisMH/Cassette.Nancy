using System;
using Cassette;
using Nancy.Responses;

namespace Nancy.Cassette
{
  namespace Nancy.Cassette
  {
    public class StaticContentHandler : ICassetteHandler
    {
      public StaticContentHandler(string handlerRoot, Func<string, Module> findModuleForPath)
      {
        this.handlerRoot = handlerRoot;
        this.findModuleForPath = findModuleForPath;
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
          Trace.Source.TraceInformation("StaticContentHandler.ProcessRequest : Module not found for path '{0}'", context.Request.Url.Path);
          return null;
        }

        var asset = module.FindAssetByPath(path);
        if (asset == null)
        {
          Trace.Source.TraceInformation("StaticContentHandler.ProcessRequest : Asset not found '{0}'", context.Request.Url.Path);
          return null;
        }

        var response = new StreamResponse(asset.OpenStream, module.ContentType);
        Trace.Source.TraceInformation("StaticContentHandler.ProcessRequest : Returned response for '{0}'", context.Request.Url.Path);
        return response;
      }

      private readonly string handlerRoot;
      private readonly Func<string, Module> findModuleForPath; 
    }
  }
}