using Cassette;
using Cassette.Utilities;

namespace Nancy.Cassette
{
  public class UrlGenerator : IUrlGenerator
  {
    public static string AssetUrlPrefix = "_assets";

    public string CreateModuleUrl(Module module)
    {
      throw new System.NotImplementedException();
    }

    public string CreateAssetUrl(IAsset asset)
    {
      return string.Format(
        "{0}/{1}?{2}",
        Hooks.Context.Request.Url.BasePath,
        asset.SourceFilename.Substring(2),
        asset.Hash.ToHexString()
        );
    }

    public string CreateAssetCompileUrl(Module module, IAsset asset)
    {
      return string.Format(
        "{0}/{1}/get/{2}?{3}",
        Hooks.Context.Request.Url.BasePath,
        AssetUrlPrefix,
        asset.SourceFilename.Substring(2),
        asset.Hash.ToHexString()
        );
    }

    public string CreateRawFileUrl(string filename, string hash)
    {
      throw new System.NotImplementedException();
    }
  }
}