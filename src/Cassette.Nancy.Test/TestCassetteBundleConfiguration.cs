using Cassette.Scripts;

namespace Cassette.Nancy.Test
{
  public class TestCassetteBundleConfiguration : IConfiguration<BundleCollection>
  {
    public void Configure(BundleCollection configurable)
    {
      //bundles.Add<StylesheetBundle>("Styles");
      configurable.AddPerSubDirectory<ScriptBundle>("Scripts");
    }
  }
}