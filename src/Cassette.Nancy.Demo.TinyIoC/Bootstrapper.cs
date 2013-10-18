using Nancy;
using Nancy.Conventions;

namespace Cassette.Nancy.Demo.TinyIoC
{
  public class Bootstrapper : DefaultNancyBootstrapper
  {
    public Bootstrapper()
    {
      CassetteNancyStartup.OptimizeOutput = false;
    }

    protected override void ApplicationStartup(global::Nancy.TinyIoc.TinyIoCContainer container, global::Nancy.Bootstrapper.IPipelines pipelines)
    {
      base.ApplicationStartup(container, pipelines);
      
      Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Images"));
    }
  }
}