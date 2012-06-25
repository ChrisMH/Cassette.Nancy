using Nancy;
using Nancy.Conventions;
using Utility.Logging;
using Utility.Logging.NLog;

namespace Cassette.Nancy.Demo.TinyIoC
{
  public class Bootstrapper : DefaultNancyBootstrapper
  {
    public Bootstrapper()
    {
      CassetteNancyStartup.OptimizeOutput = false;
    }

    protected override void ConfigureApplicationContainer(global::TinyIoC.TinyIoCContainer container)
    {
      base.ConfigureApplicationContainer(container);

      container.Register<ILoggerFactory>((c,p) => new NLogLoggerFactory());

    }
    protected override void ApplicationStartup(global::TinyIoC.TinyIoCContainer container, global::Nancy.Bootstrapper.IPipelines pipelines)
    {
      base.ApplicationStartup(container, pipelines);
      
      Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Images"));
    }
  }
}