using Nancy;

namespace Cassette.Nancy
{
  public class HudRouteHandler
  {
    public HudRouteHandler(string handlerRoot)
    {
      HandlerRoot = handlerRoot;
    }

    public Response ProcessRequest(NancyContext context)
    {
      return null;
    }

    protected string HandlerRoot { get; private set; }
  }
}