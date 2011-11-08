using System;
using System.Collections.Generic;
using System.Linq;
using Cassette;
using Cassette.HtmlTemplates;
using Cassette.Scripts;
using Cassette.Stylesheets;
using Cassette.Utilities;
using Nancy.Extensions;
using Utility.Logging;

namespace Nancy.Cassette
{
  internal class CassetteRouteHandling : IUrlGenerator
  {
    public CassetteRouteHandling(string applicationRoot, Func<NancyContext> getCurrentContext, ILogger logger)
    {
      if(string.IsNullOrEmpty(applicationRoot)) throw new ArgumentNullException("applicationRoot");
      if (getCurrentContext == null) throw new ArgumentNullException("getCurrentContext");

      this.applicationRoot = applicationRoot;
      this.getCurrentContext = getCurrentContext;
      if(logger != null) this.logger = logger.GetCurrentClassLogger();
    }
    
    #region IUrlGenerator

    public string CreateBundleUrl(Bundle bundle)
    {
      return getCurrentContext()
        .ToFullPath(string.Format("~/{0}/{1}/{2}_{3}",
                                  RoutePrefix,
                                  ConventionalBundlePathName(bundle.GetType()),
                                  bundle.Path.Substring(2),
                                  bundle.Hash.ToHexString()));
    }

    public string CreateAssetUrl(IAsset asset)
    {
      return getCurrentContext()
        .ToFullPath(string.Format("~/{0}/asset/{1}?{2}",
                                  RoutePrefix,
                                  asset.SourceFile.FullPath.Substring(2),
                                  asset.Hash.ToHexString()
                      ));
    }

    public string CreateRawFileUrl(string filename, string hash)
    {
      if (filename.StartsWith("~"))
      {
        filename = filename.Substring(2); // Remove the "~/"
      }
      var dotIndex = filename.LastIndexOf('.');
      var name = filename.Substring(0, dotIndex);
      var extension = filename.Substring(dotIndex + 1);


      return getCurrentContext()
        .ToFullPath(string.Format("~/{0}/file/{1}_{2}_{3}",
                                  RoutePrefix,
                                  ConvertToForwardSlashes(name),
                                  hash,
                                  extension
                      ));
    }

    #endregion

    internal void InstallCassetteRouteHandlers(IBundleContainer bundleContainer)
    {
      InstallBundleHandler<ScriptBundle>(bundleContainer);
      InstallBundleHandler<StylesheetBundle>(bundleContainer);
      InstallBundleHandler<HtmlTemplateBundle>(bundleContainer);
      
      InstallAssetHandler(bundleContainer);

      InstallRawFileAssetHandler(bundleContainer);
    }

    internal Response RunCassetteRouteHandler(NancyContext context)
    {
      return cassetteHandlers
        .Where(kvp => context.Request.Url.Path.StartsWith(kvp.Key, StringComparison.InvariantCultureIgnoreCase))
        .Select(kvp => kvp.Value.Invoke(context))
        .SingleOrDefault();
    }

    
    private void InstallBundleHandler<T>(IBundleContainer bundleContainer)
      where T : Bundle
    {
      var handlerRoot = string.Format(
        "/{0}/{1}",
        RoutePrefix,
        ConventionalBundlePathName(typeof (T)));

      cassetteHandlers.Add(handlerRoot, context => new BundleRouteHandler(bundleContainer, handlerRoot, logger).ProcessRequest(context));

      if (logger != null) logger.Trace("Installed Cassette route handler for '{0}'", handlerRoot);
    }


    private void InstallAssetHandler(IBundleContainer bundleContainer)
    {
      var handlerRoot = string.Format("/{0}/asset", RoutePrefix);

      cassetteHandlers.Add(handlerRoot, context => new AssetRouteHandler(bundleContainer, handlerRoot, logger).ProcessRequest(context));

      if (logger != null) logger.Trace("Installed Cassette route handler for '{0}'", handlerRoot);
    }

    
    private void InstallRawFileAssetHandler(IBundleContainer bundleContainer)
    {
      var handlerRoot = string.Format("/{0}/file", RoutePrefix);

      cassetteHandlers.Add(handlerRoot, context => new RawFileRouteHandler(bundleContainer, handlerRoot, logger, applicationRoot).ProcessRequest(context));

      if (logger != null) logger.Trace("Installed Cassette route handler for '{0}'", handlerRoot);
    }

    /*
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
    */
    
    private readonly SortedDictionary<string, Func<NancyContext, Response>> cassetteHandlers = new SortedDictionary<string, Func<NancyContext, Response>>();




    /*




    public static string GetRawFileHandlerRoot()
    {
      return string.Format(
        "/{0}/files",
        AssetUrlPrefix);
    }

    public static string GetCompiledAssetHandlerRoot()
    {
      return string.Format(
        "/{0}/get",
        AssetUrlPrefix);
    }

    public static bool RemoveUrlQuery(string url, out string urlWithoutQuery, out string query)
    {
      var queryPosition = url.IndexOf('?');
      if (queryPosition >= 0)
      {
        urlWithoutQuery = url.Remove(queryPosition);
        query = url.Substring(queryPosition + 1);
        return true;
      }

      urlWithoutQuery = url;
      query = null;
      return false;
    }
    */

    private static string ConventionalBundlePathName(Type bundleType)
    {
      // ExternalScriptBundle subclasses ScriptBundle, but we want the name to still be "scripts"
      // So walk up the inheritance chain until we get to something that directly inherits from Bundle.
      while (bundleType != null && bundleType.BaseType != typeof (Bundle))
      {
        bundleType = bundleType.BaseType;
      }
      if (bundleType == null) throw new ArgumentException("Type must be a subclass of Cassette.Bundle.", "bundleType");

      return bundleType.Name.ToLowerInvariant();
    }

    private static string ConvertToForwardSlashes(string path)
    {
      return path.Replace('\\', '/');
    }
    
    private const string RoutePrefix = "_cassette";

    private readonly Func<NancyContext> getCurrentContext;
    private readonly string applicationRoot;
    private readonly ILogger logger;
  }
}