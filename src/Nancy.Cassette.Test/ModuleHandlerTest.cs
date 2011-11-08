using System;
using HtmlAgilityPlus;
using NUnit.Framework;
using Nancy.Testing;

namespace Nancy.Cassette.Test
{
  public class ModuleHandlerTest
  {
    [TestCase("/_assets/scripts/Scripts/app")]
    [TestCase("/_assets/scripts/Scripts/lib")]
    [TestCase("/_assets/stylesheets/Styles/app")]
    public void ModuleIsReturned()
    {
      CassetteStartup.IsDebuggingEnabled = false;
      var browser = new Browser(new TestBootstrapper());
      var response = browser.Get("/RazorHome", with => with.HttpRequest());
      Console.Write(response.Body.AsString());
      
      var query = new SharpQuery(response.Body.AsString());
    } 
  }
}