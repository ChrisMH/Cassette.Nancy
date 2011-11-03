using System;
using NUnit.Framework;
using Nancy.Testing;

namespace Nancy.Cassette.Test
{
  public class RazorViewTest
  {
    [SetUp]
    public void SetUp()
    {
      browser = new Browser(new TestBootstrapper());
    }

    [Test]
    public void RazorViewContainsExpectedStylesheetReferences()
    {
      var result = browser.Get("/RazorHome", with => with.HttpRequest());

      Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
      result.Body["head link[href^='/_assets/get/Styles/app/Main.css?']"].ShouldExistOnce();
    }

    [Test]
    public void RazorViewContainsExpectedScriptReferences()
    {
      var result = browser.Get("/RazorHome", with => with.HttpRequest());

      Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
      result.Body["body script[src^='/Scripts/lib/jquery-1.6.4.js?']"].ShouldExistOnce();
      result.Body["body script[src^='/Scripts/lib/underscore.js?']"].ShouldExistOnce();
      result.Body["body script[src^='/_assets/get/Scripts/app/layout.coffee?']"].ShouldExistOnce();
    }

    private Browser browser;
  }
}