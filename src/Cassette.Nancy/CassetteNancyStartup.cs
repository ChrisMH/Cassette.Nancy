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
      this.logger = loggerFactory.GetCurrentInstanceLogger();
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
      logger.Info("Initialize");

      webHost = new WebHost(rootPathProvider, () => currentContext.Value, loggerFactory);
      webHost.Initialize();

      pipelines.BeforeRequest.AddItemToStartOfPipeline(RunCassetteRequestHandler);
      pipelines.BeforeRequest.AddItemToStartOfPipeline(InitializeCassetteRequestState);

      pipelines.AfterRequest.AddItemToEndOfPipeline(RewriteResponseContents);
    }

    private Response InitializeCassetteRequestState(NancyContext context)
    {
      //return ((CassetteApplication)CassetteApplicationContainer.Application).InitPlaceholderTracker(context);
      logger.Info("InitializeCassetteRequestState");
      return null;
    }

    private Response RunCassetteRequestHandler(NancyContext context)
    {
      currentContext.Value = context;
      //return routeGenerator.RunCassetteRouteHandler(context);
      logger.Info("RunCassetteHandler");
      return new HtmlResponse();
    }

    public void RewriteResponseContents(NancyContext context)
    {
      logger.Info("RewriteResponseContents");
      /*
      if (!context.Items.ContainsKey(CassetteApplication.PlaceholderTrackerContextKey))
      {
        // InitializePlaceholderTracker was not called for this request.  Do not attempt to rewrite.
        return;
      }

      if (!context.Response.ContentType.Equals("text/html"))
      {
        // Only html needs to be (possibly) rewritten
        return;
      }
      var currentContents = context.Response.Contents;
      context.Response.Contents =
        stream =>
        {
          //if (Logger != null)
          //  Logger.Trace("RewriteResponseContents : {0} : {1} : content type = {2}", Thread.CurrentThread.ManagedThreadId, context.Request.Url.Path,
          //    context.Response.ContentType);
          var currentContentsStream = new MemoryStream();

          currentContents(currentContentsStream);

          var reader = new StreamReader(currentContentsStream);

          //if (Logger != null)
          //{
          //  reader.BaseStream.Seek(0, SeekOrigin.Begin);
          //  Logger.Trace("RewriteResponseContents : {0} : Original Contents: \r\n {1}", Thread.CurrentThread.ManagedThreadId, reader.ReadToEnd());
          //}


          reader.BaseStream.Seek(0, SeekOrigin.Begin);
          var writer = new StreamWriter(stream);
          writer.Write(((IPlaceholderTracker)context.Items[CassetteApplication.PlaceholderTrackerContextKey]).ReplacePlaceholders(reader.ReadToEnd()));
          writer.Flush();


          //if (Logger != null)
          //{
          //  var outputReader = new StreamReader(stream);
          //  outputReader.BaseStream.Seek(0, SeekOrigin.Begin);
          //  Logger.Trace("RewriteResponseContents : {0} : Rewritten Contents: \r\n {1}", Thread.CurrentThread.ManagedThreadId, outputReader.ReadToEnd());
          //}

        };
        */
    }


    public static bool OptimizeOutput { get; set; }

    private readonly IRootPathProvider rootPathProvider; 
    private readonly ThreadLocal<NancyContext> currentContext = new ThreadLocal<NancyContext>(() => null);
    
    private WebHost webHost;

    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger logger;
  }
}