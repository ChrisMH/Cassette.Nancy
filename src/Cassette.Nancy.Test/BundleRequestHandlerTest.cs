using System;
using CsQuery;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace Cassette.Nancy.Test
{
  public class BundleRequestHandlerTest
  {
    [TestCase("head", "link", "href", "/_cassette/stylesheet/xxx=/Styles")]
    [TestCase("body", "script", "src", "/_cassette/script/xxx=/Scripts/lib")]
    [TestCase("body", "script", "src", "/_cassette/script/xxx=/Scripts/app")]
    public void BundleIsReturned(string location, string element, string attribute, string urlFragmet)
    {
      var browser = new Browser(new OptimizingBootstrapper());
      var response = browser.Get("/RazorHome", with => with.HttpRequest());
      Console.Write(response.Body.AsString());
      
      var cq = CQ.Create(response.Body.AsString());

      var url = cq.Find(string.Format("{0} {1}[{2}^='{3}']", location, element, attribute, urlFragmet)).Attr(attribute);

      response = browser.Get(url, with => with.HttpRequest());
      Console.Write(response.Body.AsString());
      Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
  }
}