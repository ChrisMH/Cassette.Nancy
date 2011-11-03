using System.Collections.Generic;
using System.Reflection;
using Cassette;
using Cassette.IO;
using Cassette.UI;
using Nancy.Bootstrapper;
using TinyIoC;
using System.Linq;

namespace Nancy.Cassette
{
  public static class Hooks
  {
    public static void Install(TinyIoCContainer container, IPipelines pipelines)
    {
      //storage = IsolatedStorageFile.GetMachineStoreForAssembly();

      configurations = container.ResolveAll<ICassetteConfiguration>();
      rootDirectory = container.Resolve<IRootPathProvider>().GetRootPath();

      //applicationContainer = ShouldOptimizeOutput() ? new CassetteApplicationContainer<CassetteApplication>(CreateCassetteApplication) 
      //                                              : new CassetteApplicationContainer<CassetteApplication>(CreateCassetteApplication, HttpRuntime.AppDomainAppPath);

      application = new CassetteApplication(
        configurations,
        new FileSystemDirectory(rootDirectory),
        null,
        null,
        false,
        GetConfigurationVersion());
        
      Assets.GetApplication = () => CassetteApplication;

      pipelines.BeforeRequest.AddItemToStartOfPipeline(application.OnBeforeRequest);
      pipelines.AfterRequest.AddItemToEndOfPipeline(application.OnAfterRequest);
    }
    
    private static string GetConfigurationVersion()
    {
      var assemblyVersion = configurations.Select(
        configuration => new AssemblyName(configuration.GetType().Assembly.FullName).Version.ToString()
        ).Distinct();

      return string.Join("|", assemblyVersion);
    }
    
    public static CassetteApplication CassetteApplication
    {
      get { return application;  }
    }

    internal static IEnumerable<ICassetteConfiguration> Configurations
    {
      get { return configurations; }
    }

    internal static string RootDirectory
    {
      get { return rootDirectory; }
    }

    private static IEnumerable<ICassetteConfiguration> configurations;
    private static string rootDirectory;
    private static CassetteApplication application;
  }
}