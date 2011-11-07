using System;
using NUnit.Framework;
using Nancy.Testing;

namespace Nancy.Cassette.Test
{
  public class RazorViewTest
  {
    [Test]
    public void RazorViewContainsExpectedStylesheetReferencesWhenOutputIsNotOptimized()
    {
      CassetteStartup.ShouldOptimizeOutput = false;
      var browser = new Browser(new TestBootstrapper());
      var result = browser.Get("/RazorHome", with => with.HttpRequest());

      Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
      Console.Write(result.Body.AsString());
      result.Body["head link[href^='/_assets/get/Styles/app/Main.css?']"].ShouldExistOnce();
    }

    [Test]
    public void RazorViewContainsExpectedScriptReferencesWhenOutputIsNotOptimized()
    {
      CassetteStartup.ShouldOptimizeOutput = false;
      var browser = new Browser(new TestBootstrapper());
      var result = browser.Get("/RazorHome", with => with.HttpRequest());

      Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
      Console.Write(result.Body.AsString());
      result.Body["body script[src^='/Scripts/fallback/jquery-1.6.4.js?']"].ShouldExistOnce();
      result.Body["body script[src^='/Scripts/lib/underscore.js?']"].ShouldExistOnce();
      result.Body["body script[src^='/_assets/get/Scripts/app/layout.coffee?']"].ShouldExistOnce();
    }

    [Test]
    public void RazorViewContainsExpectedStylesheetReferencesWhenOutputIsOptimized()
    {
      CassetteStartup.ShouldOptimizeOutput = true;
      var browser = new Browser(new TestBootstrapper());
      var result = browser.Get("/RazorHome", with => with.HttpRequest());

      Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
      Console.Write(result.Body.AsString());
      result.Body["head link[href^='/_assets/stylesheets/Styles/app_']"].ShouldExistOnce();
    }

    [Test]
    public void RazorViewContainsExpectedScriptReferencesWhenOutputIsOptimized()
    {
      CassetteStartup.ShouldOptimizeOutput = true;
      var browser = new Browser(new TestBootstrapper());
      var result = browser.Get("/RazorHome", with => with.HttpRequest());

      Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
      Console.Write(result.Body.AsString());
      result.Body["body script[src^='/_assets/scripts/Scripts/lib_']"].ShouldExistOnce();
      result.Body["body script[src^='/_assets/scripts/Scripts/app_']"].ShouldExistOnce();
    }

  }
}