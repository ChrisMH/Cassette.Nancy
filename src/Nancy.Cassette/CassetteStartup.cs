using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Threading;
using Cassette;
using Cassette.Configuration;
using Cassette.IO;
using Nancy.Bootstrapper;
using Utility.Logging;

namespace Nancy.Cassette
{
  public class CassetteStartup : IStartup
  {
    // TODO: Change to IEnumerable<ICassetteConfiguration> when I figure out how to get the types injected
    public CassetteStartup(IRootPathProvider rootPathProvider, ICassetteConfiguration cassetteConfiguration)
    {
      this.rootPathProvider = rootPathProvider;
      this.cassetteConfigurations = new List<ICassetteConfiguration> { cassetteConfiguration };
      routeHandling = new CassetteRouteHandling(rootPathProvider.GetRootPath(), GetCurrentContext, logger);
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
      var applicationRoot = rootPathProvider.GetRootPath();

      applicationContainer = ShouldOptimizeOutput ? new CassetteApplicationContainer<CassetteApplication>(CreateCassetteApplication)
                               : new CassetteApplicationContainer<CassetteApplication>(CreateCassetteApplication, applicationRoot);

      CassetteApplicationContainer.SetAccessor(() => applicationContainer.Application);

      pipelines.BeforeRequest.AddItemToStartOfPipeline(RunCassetteHandler);
      pipelines.BeforeRequest.AddItemToStartOfPipeline(InitializeCassetteRequestState);

      pipelines.AfterRequest.AddItemToEndOfPipeline(RewriteResponseContents);
    }

    private NancyContext GetCurrentContext()
    {
      return currentContext.Value;
    }

    private CassetteApplication CreateCassetteApplication()
    {
      var applicationRoot = rootPathProvider.GetRootPath();
      var cacheFile = IsolatedStorageFile.GetMachineStoreForAssembly();

      var cacheVersion = GetConfigurationVersion(cassetteConfigurations, applicationRoot);

      var settings = new CassetteSettings
                     {
                       IsDebuggingEnabled = !ShouldOptimizeOutput,
                       IsHtmlRewritingEnabled = true,
                       SourceDirectory = new FileSystemDirectory(applicationRoot),
                       CacheDirectory = new IsolatedStorageDirectory(cacheFile)
                     };

      var bundles = new BundleCollection(settings);
      foreach (var cassetteConfiguration in cassetteConfigurations)
      {
        cassetteConfiguration.Configure(bundles, settings);
      }

      if (logger != null) logger.Trace("Creating Cassette application object");
      if (logger != null) logger.Trace("IsDebuggingEnabled: {0}", settings.IsDebuggingEnabled);
      if (logger != null) logger.Trace("Cache version: {0}", cacheVersion);

      return new CassetteApplication(
        bundles,
        settings,
        routeHandling,
        cacheVersion,
        GetCurrentContext);
    }

    private static string GetConfigurationVersion(IEnumerable<ICassetteConfiguration> configurations, string applicationRoot)
    {
      var assemblyVersion = configurations.Select(
        configuration => new AssemblyName(configuration.GetType().Assembly.FullName).Version.ToString()
        ).Distinct();

      var parts = assemblyVersion.Concat(new[] {applicationRoot});
      return string.Join("|", parts);
    }

    private Response InitializeCassetteRequestState(NancyContext context)
    {
      currentContext.Value = context;
      return applicationContainer.Application.InitPlaceholderTracker(context);
    }

    private Response RunCassetteHandler(NancyContext context)
    {
      return routeHandling.RunCassetteRouteHandler(context);
    }


    public void RewriteResponseContents(NancyContext context)
    {
      if (!context.Items.ContainsKey(CassetteApplication.PlaceholderTrackerContextKey))
      {
        // InitializePlaceholderTracker was not called for this request.  Do not attempt to rewrite.
        return;
      }

      if (logger != null) logger.Trace("RewriteResponseContents : {0} : {1}", Thread.CurrentThread.ManagedThreadId, context.Request.Url.Path);

      var currentContents = context.Response.Contents;
      context.Response.Contents =
        stream =>
        {
          var currentContentsStream = new MemoryStream();
          currentContents(currentContentsStream);
          currentContentsStream.Position = 0;

          var reader = new StreamReader(currentContentsStream);

          var writer = new StreamWriter(stream);
          writer.Write(((IPlaceholderTracker) context.Items[CassetteApplication.PlaceholderTrackerContextKey]).ReplacePlaceholders(reader.ReadToEnd()));
          writer.Flush();
        };
    }

    public static bool ShouldOptimizeOutput { get; set; }

    public static ILogger Logger
    {
      set { logger = value.GetCurrentClassLogger(); }
    }

    private readonly IRootPathProvider rootPathProvider;
    private readonly List<ICassetteConfiguration> cassetteConfigurations;
    private readonly CassetteRouteHandling routeHandling;
    private readonly ThreadLocal<NancyContext> currentContext = new ThreadLocal<NancyContext>(() => null);

    private CassetteApplicationContainer<CassetteApplication> applicationContainer;

    private static ILogger logger;
  }
}