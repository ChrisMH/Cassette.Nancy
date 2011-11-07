using System;
using HtmlAgilityPlus;
using NUnit.Framework;
using Nancy.Testing;

namespace Nancy.Cassette.Test
{
  public class CompiledAssetHandlerTest
  {
    [Test]
    public void CompiledCssFileIsReturned()
    {
      var browser = new Browser(new TestBootstrapper());
      var response = browser.Get("/RazorHome", with => with.HttpRequest());
      Console.Write(response.Body.AsString());

      var query = new SharpQuery(response.Body.AsString());
      var href = query.Find("head link[href^='/_assets/get/Styles/app/Main.css?']").Attr("href");

      var result = browser.Get(href, with => with.HttpRequest());

      Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
    }

    [Test]
    public void CompiledCoffeeFileIsReturned()
    {
      var browser = new Browser(new TestBootstrapper());
      var response = browser.Get("/RazorHome", with => with.HttpRequest());
      Console.Write(response.Body.AsString());
      
      var query = new SharpQuery(response.Body.AsString());
      var href = query.Find("body script[src^='/_assets/get/Scripts/app/layout.coffee?']").Attr("src");

      var result = browser.Get(href, with => with.HttpRequest());

      Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
    } 

  }
}