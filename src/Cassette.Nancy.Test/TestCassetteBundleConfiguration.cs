using Cassette.Scripts;

namespace Cassette.Nancy.Test
{
  public class TestCassetteBundleConfiguration : IConfiguration<BundleCollection>
  {
    public void Configure(BundleCollection configurable)
    {
      //configurable.Add<StylesheetBundle>("Styles");
      configurable.AddPerSubDirectory<ScriptBundle>("Scripts");
    }
  }
}