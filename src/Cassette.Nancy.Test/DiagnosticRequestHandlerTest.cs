using System;
using HtmlAgilityPlus;
using Nancy.Testing;
using Xunit;

namespace Cassette.Nancy.Test
{
  public class DiagnosticRequestHandlerTest
  {
    [Fact]
    public void DiagnosticPageIsReturned()
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var response = browser.Get("/_cassette", with => with.HttpRequest());
      Console.Write(response.Body.AsString());

      var query = new SharpQuery(response.Body.AsString());
      var title = query.Find("head title").First();

      Assert.NotNull(title);

    }
  }
}