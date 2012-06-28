using System;
using HtmlAgilityPlus;
using Nancy;
using Nancy.Testing;
using Xunit;

namespace Cassette.Nancy.Test
{
  public class AssetRequestHandlerTest
  {
    [Fact]
    public void CssFileIsReturned()
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var response = browser.Get("/RazorHome", with => with.HttpRequest());
      Console.Write(response.Body.AsString());

      var query = new SharpQuery(response.Body.AsString());
      var url = query.Find("head link[href^='/asset/Styles/Main.css?']").Attr("href");
      
      response = browser.Get(url, with => with.HttpRequest());
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      Console.Write(response.Body.AsString());

    }

    [Fact]
    public void LessFileIsReturned()
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var response = browser.Get("/RazorHome", with => with.HttpRequest());
      Console.Write(response.Body.AsString());

      var query = new SharpQuery(response.Body.AsString());
      var url = query.Find("head link[href^='/asset/Styles/Main.less?']").Attr("href");
      
      response = browser.Get(url, with => with.HttpRequest());
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      Console.Write(response.Body.AsString());
    }

    [Fact]
    public void ScssFileIsReturned()
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var response = browser.Get("/RazorHome", with => with.HttpRequest());
      Console.Write(response.Body.AsString());

      var query = new SharpQuery(response.Body.AsString());
      var url = query.Find("head link[href^='/asset/Styles/Main.scss?']").Attr("href");

      response = browser.Get(url, with => with.HttpRequest());
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      Console.Write(response.Body.AsString());
    }

    [Fact]
    public void SassFileIsReturned()
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var response = browser.Get("/RazorHome", with => with.HttpRequest());
      Console.Write(response.Body.AsString());

      var query = new SharpQuery(response.Body.AsString());
      var url = query.Find("head link[href^='/asset/Styles/Main.sass?']").Attr("href");

      response = browser.Get(url, with => with.HttpRequest());
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      Console.Write(response.Body.AsString());
    } 


    [Fact]
    public void JsFileIsReturned()
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var response = browser.Get("/RazorHome", with => with.HttpRequest());
      Console.Write(response.Body.AsString());

      var query = new SharpQuery(response.Body.AsString());
      var url = query.Find("body script[src^='/asset/Scripts/lib/underscore.js?']").Attr("src");
      
      response = browser.Get(url, with => with.HttpRequest());
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      Console.Write(response.Body.AsString());
    }

    [Fact]
    public void CoffeeFileIsReturned()
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var response = browser.Get("/RazorHome", with => with.HttpRequest());
      Console.Write(response.Body.AsString());
      
      var query = new SharpQuery(response.Body.AsString());
      var url = query.Find("body script[src^='/asset/Scripts/app/layout.coffee?']").Attr("src");
      
      response = browser.Get(url, with => with.HttpRequest());
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      Console.Write(response.Body.AsString());
    } 

  }
}