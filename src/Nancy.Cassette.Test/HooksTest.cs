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
      var body = browser.Get("/", with => with.HttpRequest()).Body;

      var configurations = Hooks.Configurations.ToList();

      Assert.AreEqual(1, configurations.Count);
      Assert.IsInstanceOf<TestCassetteConfiguration>(configurations.First());
    }

    [Test]
    public void ResolvesRootDirectory()
    {
      var body = browser.Get("/", with => with.HttpRequest()).Body;

      Assert.AreEqual(AppDomain.CurrentDomain.BaseDirectory, Hooks.RootDirectory);
    }

    
    [Test]
    public void CreatesCassetteApplication()
    {
      var body = browser.Get("/", with => with.HttpRequest()).Body;

      Assert.NotNull(Hooks.CassetteApplication);
    }

    private Browser browser;
  }

}