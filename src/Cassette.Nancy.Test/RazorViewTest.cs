using System;
using NUnit.Framework;
using Nancy;
using Nancy.Testing;

namespace Cassette.Nancy.Test
{
  public class RazorViewTest
  {
    [Test]
    public void RazorViewContainsExpectedStylesheetReferencesWhenOutputIsNotOptimized()
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var result = browser.Get("/RazorHome", with => with.HttpRequest());

      Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
      Console.Write(result.Body.AsString());
      result.Body["head link[href^='/_cassette/asset/Styles/Main.css?']"].ShouldExistOnce();
    }

    [Test]
    public void RazorViewContainsExpectedScriptReferencesWhenOutputIsNotOptimized()
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var result = browser.Get("/RazorHome", with => with.HttpRequest());

      Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
      Console.Write(result.Body.AsString());
      result.Body["body script[src^='/_cassette/asset/Scripts/lib/jquery-1.7.js?']"].ShouldExistOnce();
      result.Body["body script[src^='/_cassette/asset/Scripts/lib/underscore.js?']"].ShouldExistOnce();
      result.Body["body script[src^='/_cassette/asset/Scripts/app/layout.coffee?']"].ShouldExistOnce();
    }

    [Test]
    public void RazorViewContainsExpectedStylesheetReferencesWhenOutputIsOptimized()
    {
      var browser = new Browser(new OptimizingBootstrapper());
      var result = browser.Get("/RazorHome", with => with.HttpRequest());

      Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
      Console.Write(result.Body.AsString());
      result.Body["head link[href^='/_cassette/stylesheetbundle/Styles_']"].ShouldExistOnce();
    }

    [Test]
    public void RazorViewContainsExpectedScriptReferencesWhenOutputIsOptimized()
    {
      var browser = new Browser(new OptimizingBootstrapper());
      var result = browser.Get("/RazorHome", with => with.HttpRequest());

      Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
      Console.Write(result.Body.AsString());
      result.Body["body script[src^='/_cassette/scriptbundle/Scripts/lib_']"].ShouldExistOnce();
      result.Body["body script[src^='/_cassette/scriptbundle/Scripts/app_']"].ShouldExistOnce();
    }

  }
}