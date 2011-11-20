using System;
using HtmlAgilityPlus;
using NUnit.Framework;
using Nancy;
using Nancy.Testing;

namespace Cassette.Nancy.Test
{
  public class BundleRouteHandlerTest
  {
    [TestCase("head", "link", "href", "/_cassette/stylesheetbundle/Styles")]
    [TestCase("body", "script", "src", "/_cassette/scriptbundle/Scripts/lib")]
    [TestCase("body", "script", "src", "/_cassette/scriptbundle/Scripts/app")]
    public void BundleIsReturned(string location, string element, string attribute, string urlFragmet)
    {
      CassetteStartup.ShouldOptimizeOutput = true;
      var browser = new Browser(new TestBootstrapper());
      var response = browser.Get("/RazorHome", with => with.HttpRequest());
      Console.Write(response.Body.AsString());
      
      var query = new SharpQuery(response.Body.AsString());

      var url = query.Find(string.Format("{0} {1}[{2}^='{3}']", location, element, attribute, urlFragmet)).Attr(attribute);
      
      response = browser.Get(url, with => with.HttpRequest());
      Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
      Console.Write(response.Body.AsString());
    }
  }
}