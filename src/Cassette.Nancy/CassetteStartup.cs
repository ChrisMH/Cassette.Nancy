using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Cassette.Configuration;
using Nancy;
using Nancy.Bootstrapper;
using Utility.Logging;

namespace Cassette.Nancy
{
  public class CassetteStartup : IStartup
  {
    public CassetteStartup(IRootPathProvider rootPathProvider)
    {
      this.rootPathProvider = rootPathProvider;

      // This will trigger creation of the Cassette infrastructure at the time of the first request.
      // The virtual directory is not known until that point, and the virtual directory is required for creation.
      this.getApplication = InitializeApplication;
      CassetteApplicationContainer.SetApplicationAccessor(getApplication);

      routeGenerator = new CassetteRouteGenerator(rootPathProvider.GetRootPath(), GetCurrentContext, Logger.GetLogger(typeof(CassetteRouteGenerator)));
    }

    public IEnumerable<TypeRegistration> TypeRegistrations
    {
      get
      {
        return null;
      }
    }

    public IEnumerable<CollectionTypeRegistration> CollectionTypeRegistrations
    {
      get
      {
        return null;
      }
    }

    public IEnumerable<InstanceRegistration> InstanceRegistrations
    {
      get
      {
        return null;
      }
    }

    public void Initialize(IPipelines pipelines)
    { 
      pipelines.BeforeRequest.AddItemToStartOfPipeline(RunCassetteHandler);
      pipelines.BeforeRequest.AddItemToStartOfPipeline(InitializeCassetteRequestState);

      pipelines.AfterRequest.AddItemToEndOfPipeline(RewriteResponseContents);
    }

    private NancyContext GetCurrentContext()
    {
      return currentContext.Value;
    }

    /*
    private CassetteApplication CreateCassetteApplication()
    {
    var applicationRoot = rootPathProvider.GetRootPath();
    var cacheFile = IsolatedStorageFile.GetMachineStoreForAssembly();

    var cassetteConfigurations = AppDomain.CurrentDomain
    .GetAssemblies()
    .SelectMany(a => a.GetTypes())
    .Where(t => typeof (ICassetteConfiguration).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
    .Select(t => (ICassetteConfiguration) Activator.CreateInstance(t))
    .ToList();

    var cacheVersion = GetConfigurationVersion(cassetteConfigurations, applicationRoot);

    var settings = new CassetteSettings(cacheVersion)
    {
    IsDebuggingEnabled = !ShouldOptimizeOutput,
    IsHtmlRewritingEnabled = true,
    SourceDirectory = new FileSystemDirectory(applicationRoot),
    CacheDirectory = new IsolatedStorageDirectory(cacheFile),
    UrlGenerator = routeHandling
    };

    var bundles = new BundleCollection(settings);

    foreach (var cassetteConfiguration in cassetteConfigurations)
    {
    cassetteConfiguration.Configure(bundles, settings);
    }

    if (Logger != null) Logger.Trace("Creating Cassette application object");
    if (Logger != null) Logger.Trace("IsDebuggingEnabled: {0}", settings.IsDebuggingEnabled);
    if (Logger != null) Logger.Trace("Cache version: {0}", cacheVersion);

    return new CassetteApplication(
    bundles,
    settings,
    routeHandling,
    GetCurrentContext);
    }

    private static string GetConfigurationVersion(IEnumerable<ICassetteConfiguration> configurations, string applicationRoot)
    {
    var assemblyVersion = configurations.Select(
    configuration => new AssemblyName(configuration.GetType().Assembly.FullName).Version.ToString()
    ).Distinct();

    var parts = assemblyVersion.Concat(new[] {applicationRoot.TrimEnd(new[] {'\\'}).Replace('\\', '_')});
    return string.Join("|", parts);
    }
    */

    private Response InitializeCassetteRequestState(NancyContext context)
    {
      currentContext.Value = context;
      return ((CassetteApplication) CassetteApplicationContainer.Application).InitPlaceholderTracker(context);
    }

    private Response RunCassetteHandler(NancyContext context)
    {
      return routeGenerator.RunCassetteRouteHandler(context);
    }

    public void RewriteResponseContents(NancyContext context)
    {
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
          if (Logger != null)
            Logger.Trace("RewriteResponseContents : {0} : {1} : content type = {2}", Thread.CurrentThread.ManagedThreadId, context.Request.Url.Path,
              context.Response.ContentType);
          var currentContentsStream = new MemoryStream();
          currentContents(currentContentsStream);
          currentContentsStream.Position = 0;

          var reader = new StreamReader(currentContentsStream);

          var writer = new StreamWriter(stream);
          writer.Write(((IPlaceholderTracker) context.Items[CassetteApplication.PlaceholderTrackerContextKey]).ReplacePlaceholders(reader.ReadToEnd()));
          writer.Flush();
        };
    }
    
    private CassetteApplication InitializeApplication()
    {
      if (currentContext.Value == null)
        throw new ApplicationException("currentContext.Value must be set before InitializeApplication is called");

      var applicationRoot = rootPathProvider.GetRootPath();
      
      var factory = new CassetteApplicationContainerFactory(
        new AssemblyScanningCassetteConfigurationFactory(AppDomain.CurrentDomain.GetAssemblies()),
        new CassetteConfigurationSection(),
        applicationRoot,
        "/",
        !ShouldOptimizeOutput,
        GetCurrentContext,
        routeGenerator);

      container = factory.CreateContainer();

      getApplication = GetApplication;
      return container.Application;
    }

    private CassetteApplication GetApplication()
    {
      if (container == null)
        throw new ApplicationException("container must be set before GetApplication is called");

      return container.Application;
    }

    private Func<CassetteApplication> getApplication;

    public static ILogger Logger { get; set; }
    public static bool ShouldOptimizeOutput { get; set; }

    private readonly IRootPathProvider rootPathProvider;
    private readonly CassetteRouteGenerator routeGenerator;
    private readonly ThreadLocal<NancyContext> currentContext = new ThreadLocal<NancyContext>(() => null);

    private CassetteApplicationContainer<CassetteApplication> container;
  }
}