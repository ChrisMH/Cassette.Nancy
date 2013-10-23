using System;
using System.Linq;
using System.Threading;
using Nancy;
using Nancy.Bootstrapper;

namespace Cassette.Nancy
{
  public class CassetteNancyStartup : IApplicationStartup
  {
    private readonly IRootPathProvider rootPathProvider;
    private readonly ThreadLocal<NancyContext> currentContext = new ThreadLocal<NancyContext>(() => null);

    private WebHost webHost;
    
    public CassetteNancyStartup(IRootPathProvider rootPathProvider)
    {
      this.rootPathProvider = rootPathProvider;
      RewriteHtml = true;
      WatchFiles = true;
    }
    
    public void Initialize(IPipelines pipelines)
    {
      pipelines.BeforeRequest.AddItemToStartOfPipeline(RunCassetteRequestHandler);

      pipelines.AfterRequest.AddItemToEndOfPipeline(RewriteResponseContents);
    }

    public static bool OptimizeOutput { get; set; }
    public static bool RewriteHtml { get; set; }
    public static bool WatchFiles { get; set; }

    private Response RunCassetteRequestHandler(NancyContext context)
    {
      currentContext.Value = context;

      // Some parts of WebHost initialization require a valid NancyContext, so defer
      // creation of the WebHost until the first request hits the pipeline.
      if(webHost == null)
      {
        webHost = new WebHost(rootPathProvider, () => currentContext.Value);
        webHost.Initialize();
      }

      return webHost.RunCassetteRequestHandler(context);
    }

    private void RewriteResponseContents(NancyContext context)
    {
      webHost.RewriteResponseContents(context);
    }

  }
}