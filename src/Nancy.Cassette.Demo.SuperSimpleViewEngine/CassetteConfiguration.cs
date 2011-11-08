using Cassette;
using Cassette.Configuration;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace Nancy.Cassette.Demo.SuperSimpleViewEngine
{
  public class CassetteConfiguration : ICassetteConfiguration
  {
    public void Configure(BundleCollection bundles, CassetteSettings settings)
    {
      bundles.Add<StylesheetBundle>("Styles");
            
      bundles.AddPerSubDirectory<ScriptBundle>("Scripts");
      bundles.AddUrl("http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.js", b => b.PageLocation = "body").WithAlias("jQuery");
            
    }
  }
}