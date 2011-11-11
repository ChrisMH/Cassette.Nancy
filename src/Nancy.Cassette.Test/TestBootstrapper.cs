using Nancy.Conventions;
using Utility.Logging.NLog;

namespace Nancy.Cassette.Test
{
  public class TestBootstrapper : DefaultNancyBootstrapper
  {
    public TestBootstrapper()
    {
      CassetteStartup.Logger = new NLogLoggerFactory().GetCurrentClassLogger();
    }

    protected override void ApplicationStartup(TinyIoC.TinyIoCContainer container, Bootstrapper.IPipelines pipelines)
    {
      base.ApplicationStartup(container, pipelines);

      Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Images"));
    }
  }
}