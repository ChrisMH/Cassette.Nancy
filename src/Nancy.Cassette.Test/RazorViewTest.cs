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
    public void TestSomethingAboutTheRazorView()
    {
      var result = browser.Get("/RazorHome", with => with.HttpRequest());

      Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
      Console.Write(result.Body.AsString());
    }

    private Browser browser;
  }
}