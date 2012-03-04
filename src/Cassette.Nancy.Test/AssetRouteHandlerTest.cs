using System;
using HtmlAgilityPlus;
using NUnit.Framework;
using Nancy;
using Nancy.Testing;

namespace Cassette.Nancy.Test
{
  public class AssetRouteHandlerTest
  {
    [Test]
    public void CssFileIsReturned()
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var response = browser.Get("/RazorHome", with => with.HttpRequest());
      Console.Write(response.Body.AsString());

      var query = new SharpQuery(response.Body.AsString());
      var url = query.Find("head link[href^='/_cassette/asset/Styles/Main.css?']").Attr("href");
      
      response = browser.Get(url, with => with.HttpRequest());
      Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
      Console.Write(response.Body.AsString());

    }

    [Test]
    public void LessFileIsReturned()
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var response = browser.Get("/RazorHome", with => with.HttpRequest());
      Console.Write(response.Body.AsString());

      var query = new SharpQuery(response.Body.AsString());
      var url = query.Find("head link[href^='/_cassette/asset/Styles/Main.less?']").Attr("href");
      
      response = browser.Get(url, with => with.HttpRequest());
      Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
      Console.Write(response.Body.AsString());

    }    
    [Test]
    public void JsFileIsReturned()
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var response = browser.Get("/RazorHome", with => with.HttpRequest());
      Console.Write(response.Body.AsString());

      var query = new SharpQuery(response.Body.AsString());
      var url = query.Find("body script[src^='/_cassette/asset/Scripts/lib/underscore.js?']").Attr("src");
      
      response = browser.Get(url, with => with.HttpRequest());
      Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
      Console.Write(response.Body.AsString());
    }

    [Test]
    public void CoffeeFileIsReturned()
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var response = browser.Get("/RazorHome", with => with.HttpRequest());
      Console.Write(response.Body.AsString());
      
      var query = new SharpQuery(response.Body.AsString());
      var url = query.Find("body script[src^='/_cassette/asset/Scripts/app/layout.coffee?']").Attr("src");
      
      response = browser.Get(url, with => with.HttpRequest());
      Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
      Console.Write(response.Body.AsString());
    } 

  }
}