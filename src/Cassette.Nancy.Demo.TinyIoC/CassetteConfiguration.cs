using Cassette;
using Cassette.Configuration;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace Cassette.Nancy.Demo.TinyIoC
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