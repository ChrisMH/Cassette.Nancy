using Cassette.Configuration;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace Cassette.Nancy.Demo.Autofac
{
  /// <summary>
  /// Configures the Cassette asset modules for the web application.
  /// </summary>
  public class CassetteConfiguration : ICassetteConfiguration
  {
    public void Configure(BundleCollection bundles, CassetteSettings settings)
    {
      bundles.Add<StylesheetBundle>("Styles");
      bundles.AddPerSubDirectory<ScriptBundle>("Scripts");
    }
  }
}