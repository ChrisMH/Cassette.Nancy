using System;
using Cassette.Stylesheets;

namespace Cassette.Nancy.Test
{
  public class TestCassetteBundleConfigurationToo : IConfiguration<BundleCollection>
  {
    public void Configure(BundleCollection configurable)
    {
      configurable.Add<StylesheetBundle>("Styles");
      //configurable.AddPerSubDirectory<ScriptBundle>("Scripts");
    }
  }
}