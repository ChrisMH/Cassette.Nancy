using System;
using CsQuery;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace Cassette.Nancy.Test
{
  public class BundleRequestHandlerTest
  {
    [TestCase("head link[href^='/_cassette/stylesheet/'][href*='/Styles']", "href")]
    [TestCase("body script[src^='/_cassette/script/'][src*='/Scripts/lib']", "src")]
    [TestCase("body script[src^='/_cassette/script/'][src*='/Scripts/app']", "src")]
    public void BundleIsReturned(string selector, string attribute) {
        var browser = new Browser(new OptimizingBootstrapper());
        var response = browser.Get("/RazorHome", with => with.HttpRequest());

        var cq = CQ.Create(response.Body.AsString());

        var url = cq.Find(selector).Attr(attribute);

        response = browser.Get(url, with => with.HttpRequest());
        Console.Write(response.Body.AsString());
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
  }
}