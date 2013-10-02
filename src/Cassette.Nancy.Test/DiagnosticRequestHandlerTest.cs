using System;
using System.Linq;
using CsQuery;
using Nancy.Testing;
using NUnit.Framework;

namespace Cassette.Nancy.Test
{
  public class DiagnosticRequestHandlerTest
  {
    [Test]
    public void DiagnosticPageIsReturned()
    {
      var browser = new Browser(new NonOptimizingBootstrapper());
      var response = browser.Get("/_cassette", with => with.HttpRequest());
      Console.Write(response.Body.AsString());

      var cq = CQ.Create(response.Body.AsString());
      var title = cq.Find("head title").First();

      Assert.NotNull(title);

    }
  }
}