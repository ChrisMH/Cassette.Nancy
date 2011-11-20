using Autofac;
using Nancy.Bootstrappers.Autofac;
using Nancy.Conventions;
using Utility.Logging;
using Utility.Logging.NLog;

namespace Cassette.Nancy.Demo.Autofac
{
  public class Bootstrapper : AutofacNancyBootstrapper
  {
    public Bootstrapper()
    {
      //CassetteStartup.ShouldOptimizeOutput = true;
      Cassette.CassetteStartup.Logger = new NLogLoggerFactory().GetLogger("CassetteStartup");
    }
    
    protected override void ApplicationStartup(ILifetimeScope container, Nancy.Bootstrapper.IPipelines pipelines)
    {
      base.ApplicationStartup(container, pipelines);
      
      Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Images"));
    }

  }
}