using System;
using NUnit.Framework;
using Nancy;
using Nancy.Testing;

namespace Cassette.Nancy.Test
{
  public class RawFileRouteHandlerTest
  {
    [TestCase("/_cassette/file/Styles/images/lorry_cffc46f6f108699377f0d4f92e88be78e31e5fcc_png")]
    public void RawImageFileIsReturned(string url)
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var response = browser.Get(url, with => with.HttpRequest());
      Console.Write(response.Body.AsString());
      
      Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
  }
}