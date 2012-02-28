using Cassette.Configuration;
using Cassette.IO;

namespace Cassette.Nancy
{
  class InitialConfiguration : ICassetteConfiguration
  {
    public InitialConfiguration(bool isDebuggingEnabled, string sourceDirectory, IUrlGenerator routeGenerator)
    {
      this.isDebuggingEnabled = isDebuggingEnabled;
      this.sourceDirectory = sourceDirectory;
      this.routeGenerator = routeGenerator;
    }

    public void Configure(BundleCollection bundles, CassetteSettings settings)
    {
      settings.IsDebuggingEnabled = isDebuggingEnabled;
      settings.IsHtmlRewritingEnabled = true;
      settings.AllowRemoteDiagnostics = false;
      settings.SourceDirectory = new FileSystemDirectory(sourceDirectory);
      settings.CacheDirectory = new IsolatedStorageDirectory(() => IsolatedStorageContainer.IsolatedStorageFile);
      settings.UrlGenerator = routeGenerator;
      settings.UrlModifier = new CassetteUrlModifier();
    }

    private readonly bool isDebuggingEnabled;
    private readonly string sourceDirectory;
    private readonly IUrlGenerator routeGenerator;
  }
}