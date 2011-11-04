using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Nancy.Testing;

namespace Nancy.Cassette.Test
{
  public class CassetteModuleTest
  {
    [SetUp]
    public void SetUp()
    {
      browser = new Browser(new TestBootstrapper());
    }

    [Test]
    public void CanGetStaticScriptFile()
    {
      var result = browser.Get("/Scripts/lib/jquery-1.6.4.js", with => with.HttpRequest());

      Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
      Assert.AreEqual("application/x-javascript", result.Context.Response.ContentType);
      
    }

    private Browser browser;
  }
}
