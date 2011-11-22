using System;
using Nancy;
using Nancy.Conventions;
using Nancy.Testing.Fakes;
using Utility.Logging.NLog;

namespace Cassette.Nancy.Test
{
  public class TestBootstrapper : DefaultNancyBootstrapper
  {
    public TestBootstrapper()
    {
      CassetteStartup.Logger = new NLogLoggerFactory().GetLogger("CassetteStartup");
    }

    protected override void ConfigureConventions(NancyConventions nancyConventions)
    {
      base.ConfigureConventions(nancyConventions);

      Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Images"));
    }

    protected override void ConfigureApplicationContainer(TinyIoC.TinyIoCContainer container)
    {
      base.ConfigureApplicationContainer(container);
      FakeRootPathProvider.RootPath = Utility.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..");
    }

    protected override void ApplicationStartup(global::TinyIoC.TinyIoCContainer container, global::Nancy.Bootstrapper.IPipelines pipelines)
    {
      base.ApplicationStartup(container, pipelines);
    }
  }
}