using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cassette;
using Cassette.IO;
using Cassette.UI;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using TinyIoC;
using Utility.Logging;

namespace Nancy.Cassette
{
  public static class Hooks
  {
    // TODO: Optimized output

    public static void Install(TinyIoCContainer container, IPipelines pipelines, NancyConventions conventions)
    {
      logger = container.Resolve<ILogger>().GetCurrentClassLogger();

      Container = container;

      //storage = IsolatedStorageFile.GetMachineStoreForAssembly();


      configurations = container.ResolveAll<ICassetteConfiguration>();
      rootDirectory = container.Resolve<IRootPathProvider>().GetRootPath();

      //applicationContainer = ShouldOptimizeOutput() ? new CassetteApplicationContainer<CassetteApplication>(CreateCassetteApplication) 
      //                                              : new CassetteApplicationContainer<CassetteApplication>(CreateCassetteApplication, HttpRuntime.AppDomainAppPath);
      

      Container.Register(CassetteApplication);
      CassetteApplication.InstallStaticPaths(conventions);

      Assets.GetApplication = () => CassetteApplication;

      pipelines.BeforeRequest.AddItemToStartOfPipeline(CassetteApplication.OnBeforeRequest);
      pipelines.AfterRequest.AddItemToEndOfPipeline(CassetteApplication.OnAfterRequest);
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
      get
      {
        return application ?? (application = new CassetteApplication(
                                               configurations,
                                               new FileSystemDirectory(rootDirectory),
                                               null,
                                               new UrlGenerator(),
                                               false,
                                               GetConfigurationVersion(),
                                               logger));
      }
    }
    
    internal static IEnumerable<ICassetteConfiguration> Configurations
    {
      get { return configurations; }
    }

    internal static string RootDirectory
    {
      get { return rootDirectory; }
    }

    internal static TinyIoC.TinyIoCContainer Container { get; private set; }

    private static IEnumerable<ICassetteConfiguration> configurations;
    private static string rootDirectory;
    private static CassetteApplication application;
    private static ILogger logger;
  }
}
