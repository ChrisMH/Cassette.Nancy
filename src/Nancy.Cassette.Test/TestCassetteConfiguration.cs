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
        });

      moduleConfiguration.Add(
        new PerSubDirectorySource<StylesheetModule>("Styles")
        {
          FilePattern = "*.css;*.less"
        });
    }
  }
}