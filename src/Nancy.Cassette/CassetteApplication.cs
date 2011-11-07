using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

      if (HtmlRewritingEnabled)
      {
        placeholderTracker = new PlaceholderTracker();
      }
      else
      {
        placeholderTracker = new NullPlaceholderTracker();
      }

      InstallCassetteHandlers();
    }

    protected override IReferenceBuilder<T> GetOrCreateReferenceBuilder<T>(Func<IReferenceBuilder<T>> create)
    {
      var key = "ReferenceBuilder:" + typeof (T).FullName;

      if(referenceBuilders.ContainsKey(key))
      {
        return (IReferenceBuilder<T>)referenceBuilders[key];
      }
      
      var referenceBuilder = create();
      referenceBuilders[key] = referenceBuilder;
      return referenceBuilder;
    }

    protected override IPlaceholderTracker GetPlaceholderTracker()
    {
      return placeholderTracker;
    }
    
    public void RewriteResponseContents(NancyContext context)
    {
      if (logger != null) logger.Info("RewriteResponseContents");

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
    
    private readonly string applicationRoot;
    private readonly List<Func<NancyContext, Response>> cassetteHandlers = new List<Func<NancyContext, Response>>();
    private readonly IPlaceholderTracker placeholderTracker;
    private readonly SortedDictionary<string,object> referenceBuilders = new SortedDictionary<string, object>(); 
    private readonly ILogger logger;
    }
}