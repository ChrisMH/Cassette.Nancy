using Nancy.Conventions;
using Utility.Logging;
using Utility.Logging.NLog;

namespace Cassette.Nancy.Demo.TinyIoC
{
  public class Bootstrapper : DefaultNancyBootstrapper
  {
    public Bootstrapper()
    {
      //CassetteStartup.ShouldOptimizeOutput = true;
      Cassette.CassetteStartup.Logger = new NLogLoggerFactory().GetLogger("CassetteStartup");
    }
    
    protected override void ApplicationStartup(global::TinyIoC.TinyIoCContainer container, global::Nancy.Bootstrapper.IPipelines pipelines)
    {
      base.ApplicationStartup(container, pipelines);
      
      Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Images"));
    }
  }
}