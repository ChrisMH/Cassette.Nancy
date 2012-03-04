using System;
using System.Linq;
using Nancy;
using Nancy.Conventions;
using Nancy.Testing.Fakes;

namespace Cassette.Nancy.Test
{
  public class OptimizingBootstrapper : DefaultNancyBootstrapper
  {
    public OptimizingBootstrapper()
    {
      CassetteStartup.ShouldOptimizeOutput = true;
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
  }
}