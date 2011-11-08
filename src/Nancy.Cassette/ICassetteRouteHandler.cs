namespace Nancy.Cassette
{
  internal interface ICassetteRouteHandler
  {
    Response ProcessRequest(NancyContext context);
  }
}