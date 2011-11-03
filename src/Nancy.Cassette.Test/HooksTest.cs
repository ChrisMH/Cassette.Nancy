using System;
using System.Linq;
using NUnit.Framework;
using Nancy.Testing;

namespace Nancy.Cassette.Test
{
  public class HooksTest
  {
    [SetUp]
    public void SetUp()
    {
      browser = new Browser(new TestBootstrapper());
    } 
    
    [Test]
    public void IdentifiesCassetteConfigurations()
    {
      browser.Get("/", with => with.HttpRequest());

      var configurations = Hooks.Configurations.ToList();

      Assert.AreEqual(1, configurations.Count);
      Assert.IsInstanceOf<TestCassetteConfiguration>(configurations.First());
    }

    [Test]
    public void ResolvesRootDirectory()
    {
      browser.Get("/", with => with.HttpRequest());

      Assert.AreEqual(AppDomain.CurrentDomain.BaseDirectory, Hooks.RootDirectory);
    }

    
    [Test]
    public void CreatesCassetteApplication()
    {
      browser.Get("/", with => with.HttpRequest());

      Assert.NotNull(Hooks.CassetteApplication);
    }

    private Browser browser;
  }

}