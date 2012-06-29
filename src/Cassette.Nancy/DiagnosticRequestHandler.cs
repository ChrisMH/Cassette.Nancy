using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Cassette.HtmlTemplates;
using Cassette.Scripts;
using Cassette.Stylesheets;
using Nancy;
using Nancy.Responses;

namespace Cassette.Nancy
{
  class DiagnosticRequestHandler : ICassetteRequestHandler
  {
    public const string PathPrefix = "/_Cassette";

    readonly BundleCollection bundles;
    readonly CassetteSettings settings;
    readonly IUrlGenerator urlGenerator;
    readonly IBundleCacheRebuilder bundleCacheRebuilder;

    public DiagnosticRequestHandler(BundleCollection bundles, CassetteSettings settings, IUrlGenerator urlGenerator, IBundleCacheRebuilder bundleCacheRebuilder)
    {
      this.bundles = bundles;
      this.settings = settings;
      this.urlGenerator = urlGenerator;
      this.bundleCacheRebuilder = bundleCacheRebuilder;
    }

    public Response ProcessRequest(NancyContext context)
    {
      if (!CanAccessHud(context))
      {
        return new HtmlResponse(HttpStatusCode.NotFound);
      }

      if (context.Request.Method.Equals("post", StringComparison.OrdinalIgnoreCase))
      {
        return ProcessPost(context);
      }

      Bundles.AddPageData("data", PageData());
      Bundles.Reference("~/Cassette.Nancy.Resources");

      var html = Properties.Resources.hud;
      html = html.Replace("$scripts$", Bundles.RenderScripts().ToHtmlString());
      return new HtmlResponse(HttpStatusCode.OK, s => 
      {
        var w = new StreamWriter(s);
        w.Write(html);
        w.Flush();
      }
        );
    }

    Response ProcessPost(NancyContext context)
    {
      if (context.Request.Form["action"] == "rebuild-cache")
      {
        bundleCacheRebuilder.RebuildCache();
      }

      return null;// new HttpResponse(HttpStatusCode.OK);
    }

    bool CanAccessHud(NancyContext context)
    {
      return true; // TODO: Check for allowing diagnostics.
      // Maybe use the Nancy diagnostics password
    }

    object PageData()
    {
      using (bundles.GetReadLock())
      {
        var scripts = bundles.OfType<ScriptBundle>();
        var stylesheets = bundles.OfType<StylesheetBundle>();
        var htmlTemplates = bundles.OfType<HtmlTemplateBundle>();

        var data = new
        {
          Scripts = scripts.Select(ScriptData),
          Stylesheets = stylesheets.Select(StylesheetData),
          HtmlTemplates = htmlTemplates.Select(HtmlTemplateData),
          //StartupTrace = CassetteHttpModule.StartUpTrace,
          Cassette = new
          {
            Version = new AssemblyName(typeof(Bundle).Assembly.FullName).Version.ToString(),
            CacheDirectory = GetCacheDirectory(settings),
            SourceDirectory = GetSourceDirectory(settings),
            settings.IsHtmlRewritingEnabled,
            settings.IsDebuggingEnabled
          }
        };
        return data;
      }
    }

    static string GetSourceDirectory(CassetteSettings settings)
    {
      if (settings.SourceDirectory == null)
        return "(none)";
      return settings.SourceDirectory.FullPath + " (" + settings.SourceDirectory.GetType().FullName + ")";
    }

    static string GetCacheDirectory(CassetteSettings settings)
    {
      if (settings.CacheDirectory == null)
        return "(none)";
      return settings.CacheDirectory.FullPath + " (" + settings.CacheDirectory.GetType().FullName + ")";
    }

    object HtmlTemplateData(HtmlTemplateBundle htmlTemplate)
    {
      return new
      {
        htmlTemplate.Path,
        Url = BundleUrl(htmlTemplate),
        Assets = AssetPaths(htmlTemplate),
        htmlTemplate.References,
        Size = BundleSize(htmlTemplate)
      };
    }

    object StylesheetData(StylesheetBundle stylesheet)
    {
      return new
      {
        stylesheet.Path,
        Url = BundleUrl(stylesheet),
        stylesheet.Media,
        stylesheet.Condition,
        Assets = AssetPaths(stylesheet),
        stylesheet.References,
        Size = BundleSize(stylesheet)
      };
    }

    object ScriptData(ScriptBundle script)
    {
      return new
      {
        script.Path,
        Url = BundleUrl(script),
        script.Condition,
        Assets = AssetPaths(script),
        script.References,
        Size = BundleSize(script)
      };
    }

    string BundleUrl(Bundle bundle)
    {
      var external = bundle as IExternalBundle;
      if (external == null)
      {
        return urlGenerator.CreateBundleUrl(bundle);
      }
      else
      {
        return external.ExternalUrl;
      }
    }

    long BundleSize(Bundle bundle)
    {
      if (settings.IsDebuggingEnabled)
      {
        return -1;
      }

      if (bundle.Assets.Any())
      {
        using (var s = bundle.OpenStream())
        {
          return s.Length;
        }
      }

      return -1;
    }

    IEnumerable<AssetLink> AssetPaths(Bundle bundle)
    {
      var generateUrls = settings.IsDebuggingEnabled;
      var visitor = generateUrls ? new AssetLinkCreator(urlGenerator) : new AssetLinkCreator();
      bundle.Accept(visitor);
      return visitor.AssetLinks;
    }

    public bool IsReusable
    {
      get { return false; }
    }

    class AssetLinkCreator : IBundleVisitor
    {
      readonly IUrlGenerator urlGenerator;
      readonly List<AssetLink> assetLinks = new List<AssetLink>();
      string bundlePath;

      public AssetLinkCreator(IUrlGenerator urlGenerator)
      {
        this.urlGenerator = urlGenerator;
      }

      public AssetLinkCreator()
      {
      }

      public List<AssetLink> AssetLinks
      {
        get { return assetLinks; }
      }

      public void Visit(Bundle bundle)
      {
        bundlePath = bundle.Path;
      }

      public void Visit(IAsset asset)
      {
        var path = asset.Path;
        if (path.Length > bundlePath.Length && path.StartsWith(bundlePath, StringComparison.OrdinalIgnoreCase))
        {
          path = path.Substring(bundlePath.Length + 1);
        }
        var url = urlGenerator != null ? urlGenerator.CreateAssetUrl(asset) : null;
        AssetLinks.Add(new AssetLink { Path = path, Url = url });
      }
    }

    class AssetLink
    {
      public string Path { get; set; }
      public string Url { get; set; }
    }
  }
}