namespace Cassette.Nancy
{
  internal interface ICassetteRouteHandler
  {
    Response ProcessRequest(NancyContext context);
  }
}