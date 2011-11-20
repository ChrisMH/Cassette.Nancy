using System;
using Nancy;
using Utility.Logging;

namespace Cassette.Nancy
{
  internal abstract class CassetteRouteHandlerBase : ICassetteRouteHandler
  {
    protected CassetteRouteHandlerBase(IBundleContainer bundleContainer, string handlerRoot, ILogger logger)
    {
      if (bundleContainer == null) throw new ArgumentNullException("bundleContainer");
      if (string.IsNullOrEmpty(handlerRoot)) throw new ArgumentNullException("handlerRoot");

      BundleContainer = bundleContainer;
      HandlerRoot = handlerRoot;
      if (logger != null) Logger = logger.GetCurrentClassLogger();
    }

    public abstract Response ProcessRequest(NancyContext context);

    protected IBundleContainer BundleContainer { get; private set; }
    protected string HandlerRoot { get; private set; }
    protected ILogger Logger { get; private set; }
  }
}