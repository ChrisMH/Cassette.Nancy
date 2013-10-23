using System;
using Nancy;
using Nancy.Testing;
using Xunit;

namespace Cassette.Nancy.Test
{
  public class RazorViewTest
  {
    [Fact]
    public void RazorViewContainsExpectedStylesheetReferencesWhenOutputIsNotOptimized()
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var result = browser.Get("/RazorHome", with => with.HttpRequest());

      Assert.Equal(HttpStatusCode.OK, result.StatusCode);
      Console.Write(result.Body.AsString());
      result.Body["head link[href^='/_cassette/asset/Styles/Main.css?']"].ShouldExistOnce();
    }

    [Fact]
    public void RazorViewContainsExpectedScriptReferencesWhenOutputIsNotOptimized()
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var result = browser.Get("/RazorHome", with => with.HttpRequest());

      Assert.Equal(HttpStatusCode.OK, result.StatusCode);
      Console.Write(result.Body.AsString());
      result.Body["body script[src^='/_cassette/asset/Scripts/lib/jquery-1.7.js?']"].ShouldExistOnce();
      result.Body["body script[src^='/_cassette/asset/Scripts/lib/underscore.js?']"].ShouldExistOnce();
      result.Body["body script[src^='/_cassette/asset/Scripts/app/layout.coffee?']"].ShouldExistOnce();
    }

    [Fact]
    public void RazorViewContainsExpectedStylesheetReferencesWhenOutputIsOptimized()
    {
      var browser = new Browser(new OptimizingBootstrapper());
      var result = browser.Get("/RazorHome", with => with.HttpRequest());

      Assert.Equal(HttpStatusCode.OK, result.StatusCode);
      Console.Write(result.Body.AsString());
      result.Body["head link[href^='/_cassette/stylesheet/'][href*='/Styles']"].ShouldExistOnce();
    }

    [Fact]
    public void RazorViewContainsExpectedScriptReferencesWhenOutputIsOptimized()
    {
      var browser = new Browser(new OptimizingBootstrapper());
      var result = browser.Get("/RazorHome", with => with.HttpRequest());

      Assert.Equal(HttpStatusCode.OK, result.StatusCode);
      Console.Write(result.Body.AsString());
      result.Body["body script[src^='/_cassette/script/'][src*='/Scripts/lib']"].ShouldExistOnce();
      result.Body["body script[src^='/_cassette/script/'][src*='/Scripts/app']"].ShouldExistOnce();
    }

  }
}