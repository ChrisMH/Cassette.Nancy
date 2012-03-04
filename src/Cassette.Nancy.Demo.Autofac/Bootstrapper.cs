using Autofac;
using Nancy.Bootstrappers.Autofac;
using Nancy.Conventions;

namespace Cassette.Nancy.Demo.Autofac
{
  public class Bootstrapper : AutofacNancyBootstrapper
  {
    public Bootstrapper()
    {
      CassetteStartup.ShouldOptimizeOutput = true;
    }
    
    protected override void ApplicationStartup(ILifetimeScope container, global::Nancy.Bootstrapper.IPipelines pipelines)
    {
      base.ApplicationStartup(container, pipelines);
      
      Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Images"));
    }

  }
}