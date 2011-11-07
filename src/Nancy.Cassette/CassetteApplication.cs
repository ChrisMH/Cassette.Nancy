using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Cassette;
using Cassette.HtmlTemplates;
using Cassette.IO;
using Cassette.Scripts;
using Cassette.Stylesheets;
using Cassette.UI;
using Cassette.Utilities;
using Nancy.Cassette.Nancy.Cassette;
using Utility.Logging;

namespace Nancy.Cassette
{
  public class CassetteApplication : CassetteApplicationBase
  {
    public CassetteApplication(IEnumerable<ICassetteConfiguration> configurations,
                               IDirectory rootDirectory, IDirectory cacheDirectory, IUrlGenerator urlGenerator,
                               bool isOutputOptimized, string version, string applicationRoot,
                               ILogger logger)
      : base(configurations, rootDirectory, cacheDirectory, urlGenerator, isOutputOptimized, version)
    {
      this.applicationRoot = applicationRoot;
      if (logger != null) this.logger = logger.GetCurrentClassLogger();
      
      InstallCassetteHandlers();
    }

    protected override IReferenceBuilder<T> GetOrCreateReferenceBuilder<T>(Func<IReferenceBuilder<T>> create)
    {
      if (currentContext.Value == null) throw new NullReferenceException("CassetteApplication.GetOrCreateReferenceBuilder : NancyContext has not been set");

      var key = "ReferenceBuilder:" + typeof (T).FullName;

      if (currentContext.Value.Items.ContainsKey(key))
      {
        return (IReferenceBuilder<T>) currentContext.Value.Items[key];
      }

      var referenceBuilder = create();
      currentContext.Value.Items[key] = referenceBuilder;
      return referenceBuilder;
    }

    protected override IPlaceholderTracker GetPlaceholderTracker()
    {
      if (currentContext.Value == null) throw new ApplicationException("CassetteApplication.GetPlaceholderTracker : NancyContext has not been set");
      if (!currentContext.Value.Items.ContainsKey(PlaceholderTrackerKey)) throw new ApplicationException("CassetteApplication.GetPlaceholderTracker : IPlaceholderTracker has not been created in the NancyContext");
      return (IPlaceholderTracker) currentContext.Value.Items[PlaceholderTrackerKey];
    }

    public NancyContext GetCurrentContext()
    {
      return currentContext.Value;
    }

    public Response InitializePlaceholderTracker(NancyContext context)
    {
      if (logger != null) logger.Trace("InitializePlaceholderTracker : {0} : {1}", Thread.CurrentThread.ManagedThreadId, context.Request.Url.Path);

      currentContext.Value = context;

      if (HtmlRewritingEnabled)
      {
        currentContext.Value.Items[PlaceholderTrackerKey] = new PlaceholderTracker();
      }
      else
      {
        currentContext.Value.Items[PlaceholderTrackerKey] = new NullPlaceholderTracker();
      }

      return null;
    }

    public Response RunCassetteHandler(NancyContext context)
    {
      return cassetteHandlers
        .Where(kvp => context.Request.Url.Path.StartsWith(kvp.Key, StringComparison.InvariantCultureIgnoreCase))
        .Select(kvp => kvp.Value.Invoke(context))
        .SingleOrDefault();
    }

    public void RewriteResponseContents(NancyContext context)
    {
      if (currentContext.Value == null)
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
          writer.Write(GetPlaceholderTracker().ReplacePlaceholders(reader.ReadToEnd()));
          writer.Flush();

          currentContext.Value = null;
        };
    }


    protected void InstallCassetteHandlers()
    {
      InstallModuleHandler<ScriptModule>();
      InstallModuleHandler<StylesheetModule>();
      InstallModuleHandler<HtmlTemplateModule>();

      InstallRawFileAssetHandler();

      InstallCompiledAssetHandler();

      InstallStaticPaths();
    }

    private void InstallModuleHandler<T>()
      where T : Module
    {
      var handlerRoot = UrlAndPathGenerator.GetModuleHandlerRoot<T>();
      cassetteHandlers.Add(handlerRoot, context => new ModuleHandler<T>(handlerRoot, GetModuleContainer<T>(), logger).ProcessRequest(context));

      if (logger != null) logger.Trace("Installed Cassette handler for '{0}'", handlerRoot);
    }

    private void InstallRawFileAssetHandler()
    {
      var handlerRoot = UrlAndPathGenerator.GetRawFileHandlerRoot();

      cassetteHandlers.Add(handlerRoot, context => new RawFileHandler(handlerRoot, applicationRoot, logger).ProcessRequest(context));

      if (logger != null) logger.Trace("Installed Cassette handler for '{0}'", handlerRoot);
    }

    private void InstallCompiledAssetHandler()
    {
      var handlerRoot = UrlAndPathGenerator.GetCompiledAssetHandlerRoot();

      cassetteHandlers.Add(handlerRoot, context => new CompiledAssetHandler(handlerRoot, FindModuleContainingPath, logger).ProcessRequest(context));

      if (logger != null) logger.Trace("Installed Cassette handler for '{0}'", handlerRoot);
    }

    protected void InstallStaticPaths()
    {
      var staticPaths = new List<string>();
      staticPaths.AddRange(GetBaseDirectories<ScriptModule>());
      staticPaths.AddRange(GetBaseDirectories<StylesheetModule>());
      staticPaths.AddRange(GetBaseDirectories<HtmlTemplateModule>());

      foreach (var staticPath in staticPaths.Distinct())
      {
        var handlerRoot = string.Concat("/", staticPath);
        cassetteHandlers.Add(handlerRoot, context => new StaticContentHandler(handlerRoot, FindModuleContainingPath, logger).ProcessRequest(context));

        if (logger != null) logger.Trace("Installed Cassette handler for '{0}'", handlerRoot);
      }
    }

    private IEnumerable<string> GetBaseDirectories<T>()
      where T : Module
    {
      return GetModuleContainer<T>()
        .Modules
        .Where(module => !module.Path.IsUrl() && !(module is ExternalScriptModule))
        .Select(module => module.Path.Split(new[] {'/'})[1]);
    }
    
    private static readonly string PlaceholderTrackerKey = typeof (IPlaceholderTracker).FullName;

    private readonly string applicationRoot;
    private readonly SortedDictionary<string, Func<NancyContext, Response>> cassetteHandlers = new SortedDictionary<string, Func<NancyContext, Response>>();
    private readonly ILogger logger;

    private ThreadLocal<NancyContext> currentContext = new ThreadLocal<NancyContext>(() => null);
  }
}