using System;
using HtmlAgilityPlus;
using NUnit.Framework;
using Nancy.Testing;

namespace Nancy.Cassette.Test
{
  public class RawFileHandlerTest
  {
    [TestCase("/_assets/files/Styles/app/images/lorry_HashDoesntMatter_png")]
    public void RawImageFileIsReturned(string path)
    {
      var browser = new Browser(new TestBootstrapper());
      var response = browser.Get(path, with => with.HttpRequest());
      Console.Write(response.Body.AsString());
      
      Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
      
    } 
  }
}