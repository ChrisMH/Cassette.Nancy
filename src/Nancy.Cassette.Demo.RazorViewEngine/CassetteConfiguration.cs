using Cassette;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace Nancy.Cassette.Demo.RazorViewEngine
{
  public class CassetteConfiguration : ICassetteConfiguration
  {
    public void Configure(ModuleConfiguration moduleConfiguration, ICassetteApplication application)
    {
      moduleConfiguration.Add(
        new ExternalScriptModule("jQuery",
                                 "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.js",
                                 "!window.jQuery",
                                 "~/Scripts/fallback/jquery-1.6.4.js"),
        new PerSubDirectorySource<ScriptModule>("Scripts")
        {
          FilePattern = "*.js;*.coffee"
        });

      moduleConfiguration.Add(
        new PerSubDirectorySource<StylesheetModule>("Styles")
        {
          FilePattern = "*.css;*.less"
        });
    }
  }
}