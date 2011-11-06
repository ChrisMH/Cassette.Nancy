namespace Nancy.Cassette
{
  public class RawFileHandler : ICassetteHandler
  {
    public RawFileHandler(string handlerRoot)
    {
      this.handlerRoot = handlerRoot;
    }


    public Response ProcessRequest(NancyContext context)
    {
      Trace.Source.TraceInformation("RawFileHandler.ProcessRequest : {0}", handlerRoot);
      return null;
    }

    private readonly string handlerRoot;
  }
}