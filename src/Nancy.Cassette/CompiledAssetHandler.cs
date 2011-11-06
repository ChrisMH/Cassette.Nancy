namespace Nancy.Cassette
{
  public class CompiledAssetHandler : ICassetteHandler
  {
    public CompiledAssetHandler(string handlerRoot)
    {
      this.handlerRoot = handlerRoot;
    }


    public Response ProcessRequest(NancyContext context)
    {
      Trace.Source.TraceInformation("CompiledAssetHandler.ProcessRequest : {0}", handlerRoot);
      return null;
    }

    private readonly string handlerRoot;
  }
}