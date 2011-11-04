using Utility.Logging;
using Utility.Logging.NLog;

namespace Nancy.Cassette.Test
{
  public class TestBootstrapper : DefaultNancyBootstrapper
  {
    protected override void ApplicationStartup(TinyIoC.TinyIoCContainer container, Bootstrapper.IPipelines pipelines)
    {
      base.ApplicationStartup(container, pipelines);

      container.Register<ILogger>((c, p) => new NLogLoggerFactory().GetLogger("Root"));

      Nancy.Cassette.Hooks.Install(container, pipelines, Conventions);
    }
  }
}