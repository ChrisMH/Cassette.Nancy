using Nancy.Conventions;
using Utility.Logging.NLog;

namespace Nancy.Cassette.Test
{
  public class TestBootstrapper : DefaultNancyBootstrapper
  {
    public TestBootstrapper()
    {
      CassetteStartup.Logger = GlobalTest.Logger;
    }

    protected override void ApplicationStartup(TinyIoC.TinyIoCContainer container, global::Nancy.Bootstrapper.IPipelines pipelines)
    {
      base.ApplicationStartup(container, pipelines);
      
      Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Images"));
    }
  }
}