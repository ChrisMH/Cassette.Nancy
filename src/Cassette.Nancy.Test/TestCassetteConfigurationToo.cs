using Cassette;
using Cassette.Configuration;
using Cassette.HtmlTemplates;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace Cassette.Nancy.Test
{
  public class TestCassetteConfigurationToo : ICassetteConfiguration
  {
    public void Configure(BundleCollection bundles, CassetteSettings settings)
    {
      bundles.Add<StylesheetBundle>("Styles");
      //bundles.AddPerSubDirectory<ScriptBundle>("Scripts");
    }
  }
}