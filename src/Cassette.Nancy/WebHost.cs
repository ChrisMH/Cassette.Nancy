using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nancy;
using Nancy.Bootstrapper;
using Utility.Logging;

namespace Cassette.Nancy
{
  public class WebHost : HostBase
  {
    public WebHost(IRootPathProvider rootPathProvider, Func<NancyContext> getContext, ILoggerFactory loggerFactory)
    {
      this.rootPathProvider = rootPathProvider;
      this.getContext = getContext;
      this.logger = loggerFactory.GetCurrentInstanceLogger();
    }

    protected override IEnumerable<Assembly> LoadAssemblies()
    {
      return AppDomainAssemblyTypeScanner.Assemblies;
    }

    protected override IConfiguration<CassetteSettings> CreateHostSpecificSettingsConfiguration()
    {
      return new WebHostSettingsConfiguration(rootPathProvider);
    }

    protected override bool CanCreateRequestLifetimeProvider
    {
      get { return true; }
    }

    protected override TinyIoC.TinyIoCContainer.ITinyIoCObjectLifetimeProvider CreateRequestLifetimeProvider()
    {
      return new NancyContextLifetimeProvider(getContext);
    }

    private readonly IRootPathProvider rootPathProvider;
    private readonly Func<NancyContext> getContext;
    private readonly ILogger logger;
  }
}
