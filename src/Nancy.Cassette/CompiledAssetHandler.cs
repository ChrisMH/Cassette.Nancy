using System;
using Cassette;
using Nancy.Responses;

namespace Nancy.Cassette
{
  public class CompiledAssetHandler : ICassetteHandler
  {
    public CompiledAssetHandler(string handlerRoot, Func<string, Module> findModuleForPath)
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
      UrlAndPathGenerator.RemoveUrlQuery(string.Concat("~", context.Request.Url.Path.Remove(0, handlerRoot.Length)), out path, out query);

      var module = findModuleForPath(path);
      if (module == null)
      {
        Trace.Source.TraceInformation("CompiledAssetHandler.ProcessRequest : Module not found for path '{0}'", context.Request.Url.Path);
        return null;
      }

      var asset = module.FindAssetByPath(path);
      if (asset == null)
      {
        Trace.Source.TraceInformation("CompiledAssetHandler.ProcessRequest : Asset not found '{0}'", context.Request.Url.Path);
        return null;
      }

      var response = new StreamResponse(asset.OpenStream, module.ContentType);
      Trace.Source.TraceInformation("CompiledAssetHandler.ProcessRequest : Returned response for '{0}'", context.Request.Url.Path);
      return response;
    }

    private readonly string handlerRoot;
    private readonly Func<string, Module> findModuleForPath; 
  }
}