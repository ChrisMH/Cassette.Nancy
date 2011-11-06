namespace Nancy.Cassette
{
  public interface ICassetteHandler
  {
    Response ProcessRequest(NancyContext context);
  }
}