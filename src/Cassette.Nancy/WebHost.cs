using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Cassette.TinyIoC;
using Nancy;
using Nancy.Bootstrapper;

namespace Cassette.Nancy
{
  public class WebHost : HostBase
  {
    private readonly IRootPathProvider rootPathProvider;
    private readonly Func<NancyContext> getContext;
    private readonly NLog.Logger logger;

    public WebHost(IRootPathProvider rootPathProvider, Func<NancyContext> getContext)
    {
      this.rootPathProvider = rootPathProvider;
      this.getContext = getContext;
      this.logger = NLog.LogManager.GetCurrentClassLogger();
    }

    public Response RunCassetteRequestHandler(NancyContext context)
    {
      logger.Info("RunCassetteRequestHandler : {0}", context.Request.Path);
      if(context.Request.Path.StartsWith(DiagnosticRequestHandler.PathPrefix))
      {
        var handler = Container.Resolve<ICassetteRequestHandler>(DiagnosticRequestHandler.PathPrefix);
        return handler.ProcessRequest(context);
      }


      if (context.Request.Path.StartsWith(AssetRequestHandler.PathPrefix))
      {
        var handler = Container.Resolve<ICassetteRequestHandler>(AssetRequestHandler.PathPrefix);
        return handler.ProcessRequest(context);
      }

      return null;
    }

    public void RewriteResponseContents(NancyContext context)
    {
      if (!context.Response.ContentType.Equals("text/html"))
      {
        // Only html needs to be (possibly) rewritten
        return;
      }
      var currentContents = context.Response.Contents;
      context.Response.Contents =
        stream =>
        { 
          var placeholderTracker = Container.Resolve<IPlaceholderTracker>();

          var currentContentsStream = new MemoryStream();
          currentContents(currentContentsStream);
          var reader = new StreamReader(currentContentsStream);

          reader.BaseStream.Seek(0, SeekOrigin.Begin);
          var writer = new StreamWriter(stream);
          
          writer.Write(placeholderTracker.ReplacePlaceholders(reader.ReadToEnd()));
          
          writer.Flush();
        };
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

    protected override void ConfigureContainer()
    {
      Container.Register<IUrlModifier>((c, p) => new UrlModifier(getContext));

      Container.Register<ICassetteRequestHandler, AssetRequestHandler>(AssetRequestHandler.PathPrefix)
               .AsPerRequestSingleton(CreateRequestLifetimeProvider());
      Container.Register<ICassetteRequestHandler, DiagnosticRequestHandler>(DiagnosticRequestHandler.PathPrefix)
               .AsPerRequestSingleton(CreateRequestLifetimeProvider());

      base.ConfigureContainer();
    }
    
  }
}