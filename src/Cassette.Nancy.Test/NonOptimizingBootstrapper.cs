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
      CassetteStartup.ShouldOptimizeOutput = false;
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