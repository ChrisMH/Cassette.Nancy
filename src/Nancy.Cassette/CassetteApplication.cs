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
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Utility.Logging;

namespace Nancy.Cassette
{
  public class CassetteApplication : CassetteApplicationBase
  {
    private readonly ILogger logger;
    private readonly SortedDictionary<string,object> referenceBuilders = new SortedDictionary<string, object>();
    private IPlaceholderTracker placeholderTracker;

    public CassetteApplication(IEnumerable<ICassetteConfiguration> configurations,
                               IDirectory rootDirectory, IDirectory cacheDirectory, IUrlGenerator urlGenerator,
                               bool isOutputOptimized, string version,
                               ILogger logger)
      : base(configurations, rootDirectory, cacheDirectory, urlGenerator, isOutputOptimized, version)
    {
      this.logger = logger.GetCurrentClassLogger();
    }


    protected override IReferenceBuilder<T> GetOrCreateReferenceBuilder<T>(Func<IReferenceBuilder<T>> create)
    {
      var key = "ReferenceBuilder:" + typeof (T).FullName;
      if (referenceBuilders.ContainsKey(key))
      {
        return (IReferenceBuilder<T>)referenceBuilders[key];
      }

      var builder = create();
      referenceBuilders[key] = builder;
      return builder;
    }

    protected override IPlaceholderTracker GetPlaceholderTracker()
    {
      return placeholderTracker;
    }


    public Response InitializePlaceholderTracker(NancyContext context)
    {
      if (HtmlRewritingEnabled)
      {
        placeholderTracker = new PlaceholderTracker();
      }
      else
      {
        placeholderTracker = new NullPlaceholderTracker();
      }

      return null;
    }

    public void RewriteResponseContents(NancyContext context)
    {
      //Logger.Info("OnAfterRequest : {0}", context.Request.Url.Path);

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

    public void InstallStaticPaths(NancyConventions conventions)
    {
      var staticPaths = new List<string>();
      staticPaths.AddRange(GetBaseDirectories<ScriptModule>());
      staticPaths.AddRange(GetBaseDirectories<StylesheetModule>());
      staticPaths.AddRange(GetBaseDirectories<HtmlTemplateModule>());

      foreach (var staticPath in staticPaths.Distinct())
      {
        if (!string.IsNullOrEmpty(staticPath))
        {
          conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory(staticPath));
          logger.Info("InstallStaticPaths : {0}", staticPath);
        }
      }
    }

    public void InstallAssetPaths()
    {
      InstallModuleAssetPath<ScriptModule>();
      InstallModuleAssetPath<StylesheetModule>();
      InstallModuleAssetPath<HtmlTemplateModule>();
    }

    private void InstallModuleAssetPath<T>()
      where T : Module
    {
      foreach (var path in GetModuleContainer<T>().Modules.Where(module => !module.Path.IsUrl()).Select(module => module.Path.TrimStart(new[] {'~', '/'})))
      {
        var handlerPath = string.Format("{0}/{1}",
                                        Nancy.Cassette.UrlGenerator.AssetUrlPrefix,
                                        path
          );

        Func<NancyContext, Response> handler =
          (context) => { return ""; };
      }
    }

    /*
    public void InstallRoutes(CassetteModule cassetteModule)
    {
      //InstallModuleRoute<ScriptModule>(cassetteModule);
      //InstallModuleRoute<StylesheetModule>(cassetteModule);
      //InstallModuleRoute<HtmlTemplateModule>(cassetteModule);

      //InstallRawFileRoute(cassetteModule);

      //InstallAssetRoute(cassetteModule);
    }
    */

    private IEnumerable<string> GetBaseDirectories<T>()
      where T : Module
    {
      return GetModuleContainer<T>()
        .Modules
        .Where(module => !module.Path.IsUrl())
        .Select(module => module.Path.Split(new[] {'/'})[1]);
    }



  }
}