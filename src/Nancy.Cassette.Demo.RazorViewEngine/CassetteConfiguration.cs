using Cassette;
using Cassette.Configuration;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace Nancy.Cassette.Demo.RazorViewEngine
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