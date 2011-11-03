namespace Nancy.Cassette.Test
{
  public class TestBootstrapper : DefaultNancyBootstrapper
  {
    protected override void ApplicationStartup(TinyIoC.TinyIoCContainer container, Bootstrapper.IPipelines pipelines)
    {
      base.ApplicationStartup(container, pipelines);

      Nancy.Cassette.Hooks.Install(container, pipelines);
    }
  }
}