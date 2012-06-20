using System;
using System.Linq;
using Cassette.IO;
using Nancy;

namespace Cassette.Nancy
{
  class WebHostSettingsConfiguration : IConfiguration<CassetteSettings>
  {
    public WebHostSettingsConfiguration(IRootPathProvider rootPathProvider)
    {
      this.rootPathProvider = rootPathProvider;
    }

    public void Configure(CassetteSettings configurable)
    {
      configurable.IsDebuggingEnabled = !CassetteNancyStartup.OptimizeOutput;
      configurable.IsHtmlRewritingEnabled = true;
      configurable.IsFileSystemWatchingEnabled = true;
      configurable.AllowRemoteDiagnostics = true;
      configurable.SourceDirectory = new FileSystemDirectory(rootPathProvider.GetRootPath());
      configurable.CacheDirectory = new IsolatedStorageDirectory(() => IsolatedStorageContainer.IsolatedStorageFile);

      // Include the virtual directory so that if the application is moved to 
      // another virtual directory the bundles will be rebuilt with the updated URLs.
      configurable.Version += rootPathProvider.GetRootPath();
    }

    private readonly IRootPathProvider rootPathProvider;
  }
}
