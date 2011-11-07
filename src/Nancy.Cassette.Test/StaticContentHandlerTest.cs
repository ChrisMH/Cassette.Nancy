using System;
using HtmlAgilityPlus;
using NUnit.Framework;
using Nancy.Testing;

namespace Nancy.Cassette.Test
{
  public class StaticContentHandlerTest
  {
    [TestCase("/Styles/app/Main.css")]
    [TestCase("/Styles/app/Main.css?HashDoesntMatter")]
    [TestCase("/Scripts/lib/underscore.js")]
    [TestCase("/Scripts/lib/underscore.js?HashDoesntMatter")]
    public void CassetteStaticContentIsReturned(string path)
    {
      var browser = new Browser(new TestBootstrapper());

      var response = browser.Get(path, with => with.HttpRequest());
      Console.Write(response.Body.AsString());

      Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestCase("/Images/tag.png")]
    public void NancyStaticContentIsReturned(string path)
    {
      var browser = new Browser(new TestBootstrapper());
      
      var response = browser.Get(path, with => with.HttpRequest());
      Console.Write(response.Body.AsString());
      
      Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
  }
}