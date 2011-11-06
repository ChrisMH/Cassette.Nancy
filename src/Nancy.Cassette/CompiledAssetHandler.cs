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
      if(!context.Request.Path.StartsWith(handlerRoot, StringComparison.InvariantCultureIgnoreCase))
      {
        return null;
      }

      
      var path = "~" + context.Request.Path.Remove(0, handlerRoot.Length);
      var query = path.IndexOf('?');
      if(query >= 0 )
      {
        path = path.Remove(query);
      }

      var module = findModuleForPath(path);
      if (module == null)
      {
        Trace.Source.TraceInformation("CompiledAssetHandler.ProcessRequest : Module not found for path '{0}'", path);
        return null;
      }

      var asset = module.FindAssetByPath(path);
      if (asset == null)
      {
        Trace.Source.TraceInformation("CompiledAssetHandler.ProcessRequest : Asset not found '{0}'", path);
        return null;
      }

      var response = new StreamResponse(asset.OpenStream, module.ContentType);
      Trace.Source.TraceInformation("CompiledAssetHandler.ProcessRequest : Returned response for '{0}'", path);
      return response;
    }

    private readonly string handlerRoot;
    private readonly Func<string, Module> findModuleForPath; 
  }
}