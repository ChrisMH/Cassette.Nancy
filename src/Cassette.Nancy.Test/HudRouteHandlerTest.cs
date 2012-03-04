using System;
using HtmlAgilityPlus;
using NUnit.Framework;
using Nancy.Testing;

namespace Cassette.Nancy.Test
{
  public class HudRouteHandlerTest
  {
  
    [Test]
    public void HudIsReturned()
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var response = browser.Get("/_cassette", with => with.HttpRequest());
      Console.Write(response.Body.AsString());

      var query = new SharpQuery(response.Body.AsString());
      var title = query.Find("head title").First();

      Assert.That(title, Is.Not.Null);

    }
  }
}