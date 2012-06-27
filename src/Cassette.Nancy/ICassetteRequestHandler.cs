using Nancy;

namespace Cassette.Nancy
{
  internal interface ICassetteRequestHandler
  {
    Response ProcessRequest(NancyContext context);
  }
}