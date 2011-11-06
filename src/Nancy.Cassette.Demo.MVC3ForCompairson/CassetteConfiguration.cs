using Cassette;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace Nancy.Cassette.Demo.MVC3ForCompairson
{
    /// <summary>
    /// Configures the Cassette asset modules for the web application.
    /// </summary>
    public class CassetteConfiguration : ICassetteConfiguration
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