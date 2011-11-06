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
      CassetteStartup.ShouldOptimizeOutput = false;
      var browser = new Browser(new DefaultNancyBootstrapper());

      var query = new SharpQuery(browser.Get("/RazorHome", with => with.HttpRequest()).Body.AsString());
      var href = query.Find("head link[href^='/_assets/get/Styles/app/Main.css?']").Attr("href");

      var result = browser.Get(href, with => with.HttpRequest());

      Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
      Console.Write(result.Body.AsString());
    }

    [Test]
    public void CompiledCoffeeFileIsReturned()
    {
      CassetteStartup.ShouldOptimizeOutput = false;
      var browser = new Browser(new DefaultNancyBootstrapper());

      var query = new SharpQuery(browser.Get("/RazorHome", with => with.HttpRequest()).Body.AsString());
      var href = query.Find("body script[src^='/_assets/get/Scripts/app/layout.coffee?']").Attr("src");

      var result = browser.Get(href, with => with.HttpRequest());

      Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
      Console.Write(result.Body.AsString());
    } 

  }
}