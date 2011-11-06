namespace Nancy.Cassette
{
  public class ModuleHandler : ICassetteHandler
  {
    public ModuleHandler(string handlerRoot)
    {
      this.handlerRoot = handlerRoot;
    }


    public Response ProcessRequest(NancyContext context)
    {
      Trace.Source.TraceInformation("ModuleHandler.ProcessRequest : {0}", handlerRoot);
      return null;
    }

    private readonly string handlerRoot;
  }
}