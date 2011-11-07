using System;
using Cassette;
using Cassette.Utilities;
using Nancy.Responses;
using Utility.Logging;

namespace Nancy.Cassette
{
  public class ModuleHandler<T> : ICassetteHandler
    where T : Module
  {
    public ModuleHandler(string handlerRoot, IModuleContainer<T> moduleContainer, ILogger logger)
    {
      this.handlerRoot = handlerRoot;
      this.moduleContainer = moduleContainer;
      if (logger != null) this.logger = logger.GetCurrentClassLogger();
    }


    public Response ProcessRequest(NancyContext context)
    {
      if (!context.Request.Url.Path.StartsWith(handlerRoot, StringComparison.InvariantCultureIgnoreCase))
      {
        return null;
      }
      
      var path = RemoveTrailingHashFromPath(string.Concat("~", context.Request.Url.Path.Remove(0, handlerRoot.Length)));

      var module = moduleContainer.FindModuleContainingPath(path);
      if (module == null)
      {
        if (logger != null) logger.Error("ModuleHandler.ProcessRequest : Module not found for path '{0}'", context.Request.Url.Path);
        return null;
      }

      /* TODO : Caching
      var actualETag = string.Concat( "\"", module.Assets[0].Hash.ToHexString(), "\"");
      var givenETag = context.Request.Headers["If-None-Match"];
      if (givenETag == actualETag)
      {
        SendNotModified(actualETag);
      }

      CacheLongTime(actualETag);

      var encoding = request.Headers["Accept-Encoding"];
      response.Filter = EncodeStreamAndAppendResponseHeaders(response.Filter, encoding);
      */

      var response = new StreamResponse(() => module.Assets[0].OpenStream(), module.ContentType);
      if (logger != null) logger.Trace("ModuleHandler.ProcessRequest : Returned response for '{0}'", context.Request.Url.Path);
      return response;
    }


    /// <summary>
    /// A Module URL has the hash appended after an underscore character. This method removes the underscore and hash from the path.
    /// </summary>
    string RemoveTrailingHashFromPath(string path)
    {
      var index = path.LastIndexOf('_');
      if (index >= 0)
      {
        return path.Substring(0, index);
      }
      return path;
    }

    private readonly string handlerRoot;
    private readonly IModuleContainer<T> moduleContainer;
    private readonly ILogger logger;
  }
}