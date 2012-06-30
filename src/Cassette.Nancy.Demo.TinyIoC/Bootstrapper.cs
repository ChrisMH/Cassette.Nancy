using Nancy;
using Nancy.Conventions;

namespace Cassette.Nancy.Demo.TinyIoC
{
  public class Bootstrapper : DefaultNancyBootstrapper
  {
    public Bootstrapper()
    {
      CassetteNancyStartup.OptimizeOutput = true;
    }

    protected override void ConfigureApplicationContainer(global::TinyIoC.TinyIoCContainer container)
    {
      base.ConfigureApplicationContainer(container);

    }
    protected override void ApplicationStartup(global::TinyIoC.TinyIoCContainer container, global::Nancy.Bootstrapper.IPipelines pipelines)
    {
      base.ApplicationStartup(container, pipelines);
      
      Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Images"));
    }
  }
}