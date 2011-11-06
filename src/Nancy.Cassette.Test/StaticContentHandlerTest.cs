using System;
using HtmlAgilityPlus;
using NUnit.Framework;
using Nancy.Testing;

namespace Nancy.Cassette.Test
{
  public class StaticContentHandlerTest
  {
    [TestCase("/Styles/app/Main.css")]
    [TestCase("/Styles/app/Main.css?QueryContentsDoesntMatter")]
    [TestCase("/Scripts/lib/underscore.js")]
    [TestCase("/Scripts/lib/underscore.js?QueryContentsDoesntMatter")]
    public void StaticContentIsReturned(string path)
    {
      CassetteStartup.ShouldOptimizeOutput = false;
      var browser = new Browser(new DefaultNancyBootstrapper());

      var result = browser.Get(path, with => with.HttpRequest());

      Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
      Console.Write(result.Body.AsString());
    }
    
  }
}