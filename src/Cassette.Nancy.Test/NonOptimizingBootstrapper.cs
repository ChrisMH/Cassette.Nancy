using System;
using Nancy;
using Nancy.Conventions;
using Nancy.Testing.Fakes;

namespace Cassette.Nancy.Test
{
  public class NonOptimizingBootstrapper : DefaultNancyBootstrapper
  {
    public NonOptimizingBootstrapper()
    {
      CassetteNancyStartup.OptimizeOutput = false;
    }

    protected override void ConfigureApplicationContainer(global::TinyIoC.TinyIoCContainer container)
    {
      base.ConfigureApplicationContainer(container);

      FakeRootPathProvider.RootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..");
    }

    protected override void ConfigureConventions(NancyConventions nancyConventions)
    {
      base.ConfigureConventions(nancyConventions);

      Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Images"));
    }
    
  }
}