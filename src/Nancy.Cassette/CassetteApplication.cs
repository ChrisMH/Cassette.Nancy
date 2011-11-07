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
      if(currentContext.Value == null) throw new NullReferenceException("CassetteApplication.GetOrCreateReferenceBuilder : NancyContext has not been set");

      var key = "ReferenceBuilder:" + typeof (T).FullName;

      if(currentContext.Value.Items.ContainsKey(key))
      {
        return (IReferenceBuilder<T>)currentContext.Value.Items[key];
      }
      
      var referenceBuilder = create();
      currentContext.Value.Items[key] = referenceBuilder;
      return referenceBuilder;
    }

    protected override IPlaceholderTracker GetPlaceholderTracker()
    {
      if(currentContext.Value == null) throw new ApplicationException("CassetteApplication.GetPlaceholderTracker : NancyContext has not been set");
      if(!currentContext.Value.Items.ContainsKey(PlaceholderTrackerKey)) throw new ApplicationException("CassetteApplication.GetPlaceholderTracker : IPlaceholderTracker has not been created in the NancyContext");
      return (IPlaceholderTracker)currentContext.Value.Items[PlaceholderTrackerKey];
    }
    
    public Response InitializePlaceholderTracker(NancyContext context)
    {
      if (logger != null) logger.Info("InitializePlaceholderTracker : {0}", Thread.CurrentThread.ManagedThreadId);

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

    public void RewriteResponseContents(NancyContext context)
    {
      if (logger != null) logger.Info("RewriteResponseContents : {0}", Thread.CurrentThread.ManagedThreadId);

      var currentContents = context.Response.Contents;
      context.Response.Contents =
        stream =>
        {
          var currentContentsStream = new MemoryStream();
          currentContents(currentContentsStream);
          currentContentsStream.Position = 0;

          var reader = new StreamReader(currentContentsStream);

          var html = reader.ReadToEnd();

          html = GetPlaceholderTracker().ReplacePlaceholders(html);

          var writer = new StreamWriter(stream);
          writer.Write(html);

          writer.Flush();
        };
    }

    public Response RunCassetteHandlers(NancyContext context)
    {
      return cassetteHandlers
        .Select(cassetteHandler => cassetteHandler.Invoke(context))
        .FirstOrDefault(response => response != null);
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
      cassetteHandlers.Add(context => new ModuleHandler<T>(handlerRoot, GetModuleContainer<T>(), logger).ProcessRequest(context));

      if (logger != null) logger.Info("Installed Cassette handler for '{0}'", handlerRoot);
    }

    private void InstallRawFileAssetHandler()
    {
      var handlerRoot = UrlAndPathGenerator.GetRawFileHandlerRoot();

      cassetteHandlers.Add(context => new RawFileHandler(handlerRoot, applicationRoot, logger).ProcessRequest(context));

      if (logger != null) logger.Info("Installed Cassette handler for '{0}'", handlerRoot);
    }

    private void InstallCompiledAssetHandler()
    {
      var handlerRoot = UrlAndPathGenerator.GetCompiledAssetHandlerRoot();

      cassetteHandlers.Add(context => new CompiledAssetHandler(handlerRoot, FindModuleContainingPath, logger).ProcessRequest(context));

      if (logger != null) logger.Info("Installed Cassette handler for '{0}'", handlerRoot);
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
        cassetteHandlers.Add(context => new StaticContentHandler(handlerRoot, FindModuleContainingPath, logger).ProcessRequest(context));

        if (logger != null) logger.Info("Installed Cassette handler for '{0}'", handlerRoot);
      }
    }


    private IEnumerable<string> GetBaseDirectories<T>()
      where T : Module
    {
      return GetModuleContainer<T>()
        .Modules
        .Where(module => !module.Path.IsUrl())
        .Select(module => module.Path.Split(new[] {'/'})[1]);
    }

    private static readonly string PlaceholderTrackerKey = typeof (IPlaceholderTracker).FullName;

    private readonly string applicationRoot;
    private readonly List<Func<NancyContext, Response>> cassetteHandlers = new List<Func<NancyContext, Response>>();
    private readonly ILogger logger;

    private readonly ThreadLocal<NancyContext> currentContext = new ThreadLocal<NancyContext>(() => null);
  }
}