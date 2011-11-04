using System;
using Cassette;
using Cassette.Utilities;

namespace Nancy.Cassette
{
  public class UrlGenerator : IUrlGenerator
  {
    public static string AssetUrlPrefix = "_assets";

    public string CreateModuleUrl(Module module)
    {
      return string.Format("{0}/{1}/{2}/{3}_{4}",
                           Hooks.CassetteApplication.Context.Request.Url.BasePath,
                           AssetUrlPrefix,
                           ConventionalModulePathName(module.GetType()),
                           module.Path.Substring(2),
                           module.Assets[0].Hash.ToHexString()
        );
    }

    public string CreateAssetUrl(IAsset asset)
    {
      return string.Format(
        "{0}/{1}?{2}",
        Hooks.CassetteApplication.Context.Request.Url.BasePath,
        asset.SourceFilename.Substring(2),
        asset.Hash.ToHexString()
        );
    }

    public string CreateAssetCompileUrl(Module module, IAsset asset)
    {
      return string.Format(
        "{0}/{1}/get/{2}?{3}",
        Hooks.CassetteApplication.Context.Request.Url.BasePath,
        AssetUrlPrefix,
        asset.SourceFilename.Substring(2),
        asset.Hash.ToHexString()
        );
    }

    public string CreateRawFileUrl(string filename, string hash)
    {
      if (filename.StartsWith("~") == false)
      {
        throw new ArgumentException("Filename must be application relative (starting with '~').");
      }

      filename = filename.Substring(2); // Remove the "~/"
      var dotIndex = filename.LastIndexOf('.');
      var name = filename.Substring(0, dotIndex);
      var extension = filename.Substring(dotIndex + 1);

      return string.Format("{0}/{1}/files/{2}_{3}_{4}",
                           Hooks.CassetteApplication.Context.Request.Url.BasePath,
                           AssetUrlPrefix,
                           ConvertToForwardSlashes(name),
                           hash,
                           extension
        );
    }

    public static string GetModuleStaticPath(Module module)
    {
      if(module.Path.IsUrl())
      {
        return null;
      }
      return ConvertToBackSlashes(module.Path.TrimStart(new[] { '~', '/' }));
    }

    public static string GetModuleRouteUrl<T>()
    {
      return string.Format(
        "{0}/{1}/{{path}}",
        AssetUrlPrefix,
        ConventionalModulePathName(typeof (T))
        );
    }

    public static string GetAssetRouteUrl()
    {
      return AssetUrlPrefix + "/get/{path}";
    }

    public static string GetRawFileRouteUrl()
    {
      return AssetUrlPrefix + "/files/{path}";
    }

    private static string ConventionalModulePathName(Type moduleType)
    {
      // ExternalScriptModule subclasses ScriptModule, but we want the name to still be "scripts"
      // So walk up the inheritance chain until we get to something that directly inherits from Module.
      while (moduleType.BaseType != typeof (Module))
      {
        moduleType = moduleType.BaseType;
      }

      var name = moduleType.Name;
      name = name.Substring(0, name.Length - "Module".Length);
      return name.ToLowerInvariant() + "s";
    }

    private static string ConvertToForwardSlashes(string path)
    {
      return path.Replace('\\', '/');
    }

    private static string ConvertToBackSlashes(string path)
    {
      return path.Replace('/', '\\');
    }
  }
}