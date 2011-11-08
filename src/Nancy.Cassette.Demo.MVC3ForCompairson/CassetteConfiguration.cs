using Cassette.Configuration;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace Nancy.Cassette.Demo.MVC3ForCompairson
{
  public class CassetteConfiguration : ICassetteConfiguration
  {
    public void Configure(BundleCollection bundles, CassetteSettings settings)
    {
      bundles.Add<StylesheetBundle>("Styles");
      bundles.AddPerSubDirectory<ScriptBundle>("Scripts");
    }
  }
}