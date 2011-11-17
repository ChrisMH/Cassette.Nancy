using System;
using System.ComponentModel;
using Cassette.Configuration;
using Nancy.Conventions;
using Utility.Logging.NLog;

namespace Nancy.Cassette.Test
{
  public class TestBootstrapper : DefaultNancyBootstrapper
  {
    public TestBootstrapper()
    {
      Cassette.CassetteStartup.Logger = new NLogLoggerFactory().GetLogger("CassetteStartup");
    }

    protected override void ApplicationStartup(TinyIoC.TinyIoCContainer container, Bootstrapper.IPipelines pipelines)
    {
      base.ApplicationStartup(container, pipelines);

      Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Images"));
    }
  }
}