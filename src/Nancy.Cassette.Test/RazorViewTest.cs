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
      CassetteStartup.IsDebuggingEnabled = true;
      var browser = new Browser(new TestBootstrapper());
      var result = browser.Get("/RazorHome", with => with.HttpRequest());

      Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
      Console.Write(result.Body.AsString());
      result.Body["head link[href^='/_cassette/asset/Styles/Main.css?']"].ShouldExistOnce();
    }

    [Test]
    public void RazorViewContainsExpectedScriptReferencesWhenOutputIsNotOptimized()
    {
      CassetteStartup.IsDebuggingEnabled = true;
      var browser = new Browser(new TestBootstrapper());
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
      CassetteStartup.IsDebuggingEnabled = false;
      var browser = new Browser(new TestBootstrapper());
      var result = browser.Get("/RazorHome", with => with.HttpRequest());

      Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
      Console.Write(result.Body.AsString());
      result.Body["head link[href^='/_cassette/stylesheetbundle/Styles_']"].ShouldExistOnce();
    }

    [Test]
    public void RazorViewContainsExpectedScriptReferencesWhenOutputIsOptimized()
    {
      CassetteStartup.IsDebuggingEnabled = false;
      var browser = new Browser(new TestBootstrapper());
      var result = browser.Get("/RazorHome", with => with.HttpRequest());

      Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
      Console.Write(result.Body.AsString());
      result.Body["body script[src^='/_cassette/scriptbundle/Scripts/lib_']"].ShouldExistOnce();
      result.Body["body script[src^='/_cassette/scriptbundle/Scripts/app_']"].ShouldExistOnce();
    }

  }
}