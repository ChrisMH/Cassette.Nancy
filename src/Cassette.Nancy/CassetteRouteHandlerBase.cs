using System;
using Nancy;

namespace Cassette.Nancy
{
  internal abstract class CassetteRouteHandlerBase : ICassetteRouteHandler
  {
    protected CassetteRouteHandlerBase(IBundleContainer bundleContainer, string handlerRoot)
    {
      if (bundleContainer == null) throw new ArgumentNullException("bundleContainer");
      if (string.IsNullOrEmpty(handlerRoot)) throw new ArgumentNullException("handlerRoot");

      BundleContainer = bundleContainer;
      HandlerRoot = handlerRoot;
    }

    public abstract Response ProcessRequest(NancyContext context);

    protected IBundleContainer BundleContainer { get; private set; }
    protected string HandlerRoot { get; private set; }
  }
}