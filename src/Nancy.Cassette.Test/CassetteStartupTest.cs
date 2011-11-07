using System.Linq;
using NUnit.Framework;
using Nancy.Testing;
using TinyIoC;

namespace Nancy.Cassette.Test
{
  public class CassetteStartupTest
  {
    /*
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
    */

    [Test]
    public void CassetteApplicationCreatedAndRegistered()
    {
      var browser = new Browser(new TestBootstrapper());

      var result = browser.Get("/", with => with.HttpRequest());

      var container = (TinyIoCContainer)result.Context.Items.Values.Single(value => value is TinyIoCContainer);
      
      object application = null;
      Assert.True(container.TryResolve(typeof(CassetteApplication), out application));
      Assert.IsInstanceOf<CassetteApplication>(application);
      Assert.NotNull(application);
    }
  }
}