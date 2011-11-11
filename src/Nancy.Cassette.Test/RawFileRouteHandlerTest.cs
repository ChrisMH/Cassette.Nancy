using System;
using HtmlAgilityPlus;
using NUnit.Framework;
using Nancy.Testing;

namespace Nancy.Cassette.Test
{
  public class RawFileRouteHandlerTest
  {
    [TestCase("/_cassette/file/Styles/images/lorry_cffc46f6f108699377f0d4f92e88be78e31e5fcc_png")]
    public void RawImageFileIsReturned(string url)
    {
      var browser = new Browser(new TestBootstrapper());
      var response = browser.Get(url, with => with.HttpRequest());
      Console.Write(response.Body.AsString());
      
      Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
      
    } 
  }
}