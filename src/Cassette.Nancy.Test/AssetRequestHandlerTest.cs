using System;
using CsQuery;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace Cassette.Nancy.Test
{
  public class AssetRequestHandlerTest
  {
    [Test]
    public void CssFileIsReturned()
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var response = browser.Get("/RazorHome", with => with.HttpRequest());
      Console.Write(response.Body.AsString());

      var cq = CQ.Create(response.Body.AsString());
      var url = cq.Find("head link[href^='/_cassette/asset/Styles/Main.css?']").Attr("href").Split(new char[] { '?' });

      response = browser.Get(url[0], with => with.HttpRequest());
      Console.Write(response.Body.AsString());
      Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

    }

    [Test]
    public void LessFileIsReturned()
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var response = browser.Get("/RazorHome", with => with.HttpRequest());
      Console.Write(response.Body.AsString());

      var cq = CQ.Create(response.Body.AsString());
      var url = cq.Find("head link[href^='/_cassette/asset/Styles/Main.less?']").Attr("href").Split(new char[] { '?' });

      response = browser.Get(url[0], with => with.HttpRequest());
      Console.Write(response.Body.AsString());
      Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [Test]
    public void ScssFileIsReturned()
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var response = browser.Get("/RazorHome", with => with.HttpRequest());
      Console.Write(response.Body.AsString());

      var cq = CQ.Create(response.Body.AsString());
      var url = cq.Find("head link[href^='/_cassette/asset/Styles/Main.scss?']").Attr("href").Split(new char[] { '?' });

      response = browser.Get(url[0], with => with.HttpRequest());
      Console.Write(response.Body.AsString());
      Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [Test]
    public void SassFileIsReturned()
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var response = browser.Get("/RazorHome", with => with.HttpRequest());
      Console.Write(response.Body.AsString());

      var cq = CQ.Create(response.Body.AsString());
      var url = cq.Find("head link[href^='/_cassette/asset/Styles/Main.sass?']").Attr("href").Split(new char[] { '?' });

      response = browser.Get(url[0], with => with.HttpRequest());
      Console.Write(response.Body.AsString());
      Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    } 


    [Test]
    public void JsFileIsReturned()
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var response = browser.Get("/RazorHome", with => with.HttpRequest());
      Console.Write(response.Body.AsString());

      var cq = CQ.Create(response.Body.AsString());
      var url = cq.Find("body script[src^='/_cassette/asset/Scripts/lib/underscore.js?']").Attr("src").Split(new char[] { '?' });

      response = browser.Get(url[0], with => with.HttpRequest());
      Console.Write(response.Body.AsString());
      Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [Test]
    public void CoffeeFileIsReturned()
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var response = browser.Get("/RazorHome", with => with.HttpRequest());
      Console.Write(response.Body.AsString());
      
      var cq = CQ.Create(response.Body.AsString());
      var url = cq.Find("body script[src^='/_cassette/asset/Scripts/app/layout.coffee?']").Attr("src").Split(new char[] { '?' });

      response = browser.Get(url[0], with => with.HttpRequest());
      Console.Write(response.Body.AsString());
      Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    } 

  }
}