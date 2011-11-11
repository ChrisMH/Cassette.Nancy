using Nancy.Conventions;

namespace Nancy.Cassette.Test
{
  public class TestBootstrapper : DefaultNancyBootstrapper
  {
    public TestBootstrapper()
    {
      CassetteStartup.Logger = GlobalTest.Logger;
    }

    protected override void ApplicationStartup(TinyIoC.TinyIoCContainer container, Bootstrapper.IPipelines pipelines)
    {
      base.ApplicationStartup(container, pipelines);

      Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Images"));
    }
  }
}