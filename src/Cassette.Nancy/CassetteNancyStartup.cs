using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Responses;
using Utility.Logging;

namespace Cassette.Nancy
{
  public class CassetteNancyStartup : IStartup
  {

    public CassetteNancyStartup(IRootPathProvider rootPathProvider, ILoggerFactory loggerFactory)
    {
      this.rootPathProvider = rootPathProvider;
      this.loggerFactory = loggerFactory;
    }

    public IEnumerable<TypeRegistration> TypeRegistrations
    {
      get { return null; }
    }

    public IEnumerable<CollectionTypeRegistration> CollectionTypeRegistrations
    {
      get { return null; }
    }

    public IEnumerable<InstanceRegistration> InstanceRegistrations
    {
      get { return null; }
    }

    public void Initialize(IPipelines pipelines)
    {
      pipelines.BeforeRequest.AddItemToStartOfPipeline(RunCassetteRequestHandler);

      pipelines.AfterRequest.AddItemToEndOfPipeline(RewriteResponseContents);
    }
    
    private Response RunCassetteRequestHandler(NancyContext context)
    {
      currentContext.Value = context;

      // Some parts of WebHost initialization require a valid NancyContext, so defer
      // creation of the WebHost until the first request hits the pipeline.
      if(webHost == null)
      {
        webHost = new WebHost(rootPathProvider, () => currentContext.Value, loggerFactory);
        webHost.Initialize();
      }

      return webHost.RunCassetteRequestHandler(context);
    }

    public void RewriteResponseContents(NancyContext context)
    {
      webHost.RewriteResponseContents(context);
    }

    public static bool OptimizeOutput { get; set; }

    private readonly IRootPathProvider rootPathProvider; 
    private readonly ThreadLocal<NancyContext> currentContext = new ThreadLocal<NancyContext>(() => null);
    
    private WebHost webHost;

    private readonly ILoggerFactory loggerFactory;
  }
}