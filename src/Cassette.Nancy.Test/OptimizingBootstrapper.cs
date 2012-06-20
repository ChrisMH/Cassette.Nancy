using System;
using System.Linq;
using Nancy;
using Nancy.Conventions;
using Nancy.Testing.Fakes;
using Utility.Logging;
using Utility.Logging.NLog;

namespace Cassette.Nancy.Test
{
  public class OptimizingBootstrapper : DefaultNancyBootstrapper
  {
    public OptimizingBootstrapper()
    {
      CassetteNancyStartup.OptimizeOutput = false;
    }

    protected override void ConfigureApplicationContainer(global::TinyIoC.TinyIoCContainer container)
    {
      base.ConfigureApplicationContainer(container);
      container.Register<ILoggerFactory>((c, p) => new NLogLoggerFactory());
      FakeRootPathProvider.RootPath = Utility.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..");
    }

    protected override void ConfigureConventions(NancyConventions nancyConventions)
    {
      base.ConfigureConventions(nancyConventions);

      Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Images"));
    }

  }
}