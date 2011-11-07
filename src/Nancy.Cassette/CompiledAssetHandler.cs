using System;
using Cassette;
using Nancy.Responses;
using Utility.Logging;

namespace Nancy.Cassette
{
  public class CompiledAssetHandler : ICassetteHandler
  {
    public CompiledAssetHandler(string handlerRoot, Func<string, Module> findModuleForPath, ILogger logger)
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
      UrlAndPathGenerator.RemoveUrlQuery(string.Concat("~", context.Request.Url.Path.Remove(0, handlerRoot.Length)), out path, out query);

      var module = findModuleForPath(path);
      if (module == null)
      {
        if (logger != null) logger.Error("CompiledAssetHandler.ProcessRequest : Module not found for path '{0}'", context.Request.Url.Path);
        return null;
      }

      var asset = module.FindAssetByPath(path);
      if (asset == null)
      {
        if (logger != null) logger.Error("CompiledAssetHandler.ProcessRequest : Asset not found '{0}'", context.Request.Url.Path);
        return null;
      }

      var response = new StreamResponse(asset.OpenStream, module.ContentType);
      if (logger != null) logger.Trace("CompiledAssetHandler.ProcessRequest : Returned response for '{0}'", context.Request.Url.Path);
      return response;
    }

    private readonly string handlerRoot;
    private readonly Func<string, Module> findModuleForPath;
    private readonly ILogger logger;
  }
}