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
    private readonly object webHostLocker = new object();
    private readonly NLog.Logger logger;
    private WebHost webHost;
    
    public CassetteNancyStartup(IRootPathProvider rootPathProvider)
    {
      this.rootPathProvider = rootPathProvider;
      this.logger = NLog.LogManager.GetCurrentClassLogger();
    }
    
    public void Initialize(IPipelines pipelines)
    {
      pipelines.BeforeRequest.AddItemToStartOfPipeline(RunCassetteRequestHandler);

      pipelines.AfterRequest.AddItemToEndOfPipeline(RewriteResponseContents);
    }

    public static bool OptimizeOutput { get; set; }
    
    private Response RunCassetteRequestHandler(NancyContext context)
    {
      logger.Debug("RunCassetteRequestHandler starting");
      currentContext.Value = context;

      // Some parts of WebHost initialization require a valid NancyContext, so defer
      // creation of the WebHost until the first request hits the pipeline.
      if(webHost == null)
      {
          logger.Debug("WebHost is null, acquiring lock...");

          lock (webHostLocker) {
              logger.Debug("I have the lock...");

              if (webHost == null) {
                  logger.Debug("I have the lock and webHost is still null. Creating/initing it...");
                  webHost = new WebHost(rootPathProvider, () => currentContext.Value);
                  webHost.Initialize();
                  logger.Debug("...finished creating/initing webHost.");
              }
          }
      }

      logger.Debug("RunCassetteRequestHandler - running request handler.");
      return webHost.RunCassetteRequestHandler(context);
    }

    private void RewriteResponseContents(NancyContext context)
    {
      webHost.RewriteResponseContents(context);
    }

  }
}