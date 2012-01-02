using Nancy;
using Utility.Logging;

namespace Cassette.Nancy
{
  public class HudRouteHandler
  {
    public HudRouteHandler(string handlerRoot, ILogger logger)
    {
      HandlerRoot = handlerRoot;
      Logger = logger;
    }

    public Response ProcessRequest(NancyContext context)
    {
      return null;
    }

    protected string HandlerRoot { get; private set; }
    protected ILogger Logger { get; private set; }
  }
}