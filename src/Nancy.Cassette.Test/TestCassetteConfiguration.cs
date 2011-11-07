using Cassette;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace Nancy.Cassette.Test
{
  public class TestCassetteConfiguration : ICassetteConfiguration
  {
    public void Configure(ModuleConfiguration moduleConfiguration, ICassetteApplication application)
    {
      moduleConfiguration.Add(
        new PerSubDirectorySource<ScriptModule>("Scripts")
        {
          FilePattern = "*.js;*.coffee"
        },
        new ExternalScriptModule("jQuery",
                                 "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.js",
                                 "!window.jQuery",
                                 "~/Scripts/fallback/jquery-1.6.4.js")
        );
        
      moduleConfiguration.Add(
        new PerSubDirectorySource<StylesheetModule>("Styles")
        {
          FilePattern = "*.css;*.less"
        });
    }
  }
}