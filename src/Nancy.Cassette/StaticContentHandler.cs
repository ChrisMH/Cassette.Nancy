namespace Nancy.Cassette
{
  namespace Nancy.Cassette
  {
    public class StaticContentHandler : ICassetteHandler
    {
      public StaticContentHandler(string handlerRoot)
      {
        this.handlerRoot = handlerRoot;
      }

      public Response ProcessRequest(NancyContext context)
      {
        Trace.Source.TraceInformation("StaticContentHandler.ProcessRequest : {0}", handlerRoot);
        return null;
      }

      private readonly string handlerRoot;
    }
  }
}