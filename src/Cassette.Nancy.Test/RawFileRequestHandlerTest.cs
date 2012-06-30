using System;
using Nancy;
using Nancy.Testing;

namespace Cassette.Nancy.Test
{
  public class RawFileRequestHandlerTest
  {
    [Theory]
    [InlineData("/_cassette/file/Styles/images/lorry-cffc46f6f108699377f0d4f92e88be78e31e5fcc.png")]
    public void RawImageFileIsReturned(string url)
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var response = browser.Get(url, with => with.HttpRequest());
      Console.Write(response.Body.AsString());
      
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
  }
}