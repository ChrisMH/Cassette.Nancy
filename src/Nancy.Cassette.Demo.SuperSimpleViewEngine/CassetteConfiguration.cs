using Cassette;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace Nancy.Cassette.Demo.SuperSimpleViewEngine
{
  public class CassetteConfiguration : ICassetteConfiguration
  {
    public void Configure(ModuleConfiguration moduleConfiguration, ICassetteApplication application)
    {
      moduleConfiguration.Add(
        new DirectorySource<ScriptModule>("Scripts")
        {
          FilePattern = "*.js;*.coffee"
        });

      moduleConfiguration.Add(
        new DirectorySource<StylesheetModule>("Styles")
        {
          FilePattern = "*.css;*.less"
        });
    }
  }
}