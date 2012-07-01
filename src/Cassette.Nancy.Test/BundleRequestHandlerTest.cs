using System;
using HtmlAgilityPlus;
using Nancy;
using Nancy.Testing;
using Xunit;
using Xunit.Extensions;

namespace Cassette.Nancy.Test
{
  public class BundleRequestHandlerTest
  {
    [Theory]
    [InlineData("head", "link", "href", "/_cassette/stylesheet/xxx=/Styles")]
    [InlineData("body", "script", "src", "/_cassette/script/xxx=/Scripts/lib")]
    [InlineData("body", "script", "src", "/_cassette/script/xxx=/Scripts/app")]
    public void BundleIsReturned(string location, string element, string attribute, string urlFragmet)
    {
      var browser = new Browser(new OptimizingBootstrapper());
      var response = browser.Get("/RazorHome", with => with.HttpRequest());
      Console.Write(response.Body.AsString());
      
      var query = new SharpQuery(response.Body.AsString());

      var url = query.Find(string.Format("{0} {1}[{2}^='{3}']", location, element, attribute, urlFragmet)).Attr(attribute);

      response = browser.Get(url, with => with.HttpRequest());
      Console.Write(response.Body.AsString());
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
  }
}