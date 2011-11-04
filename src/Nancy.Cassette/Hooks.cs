using System;
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
    // TODO: Caching

    public static void Install(TinyIoCContainer container, IPipelines pipelines, NancyConventions conventions)
    {
      logger = container.Resolve<ILogger>().GetCurrentClassLogger();

      var configurations = container.ResolveAll<ICassetteConfiguration>().ToList();
      var rootDirectory = container.Resolve<IRootPathProvider>().GetRootPath();

      //storage = IsolatedStorageFile.GetMachineStoreForAssembly();

      //applicationContainer = ShouldOptimizeOutput() ? new CassetteApplicationContainer<CassetteApplication>(CreateCassetteApplication) 
      //                                              : new CassetteApplicationContainer<CassetteApplication>(CreateCassetteApplication, HttpRuntime.AppDomainAppPath);



      CassetteApplication = new CassetteApplication(
        configurations,
        new FileSystemDirectory(rootDirectory),
        null,
        new UrlGenerator(),
        false, // TODO: change output optimized flag
        GetConfigurationVersion(configurations),
        logger);
      CassetteApplication.InstallStaticPaths(conventions);
      CassetteApplication.InstallAssetPaths();

      pipelines.BeforeRequest.AddItemToStartOfPipeline(CassetteApplication.InitializePlaceholderTracker);
      pipelines.AfterRequest.AddItemToEndOfPipeline(CassetteApplication.RewriteResponseContents);

      Assets.GetApplication = () => CassetteApplication;
    }
    
    private static string GetConfigurationVersion(IEnumerable<ICassetteConfiguration> configurations)
    {
      var assemblyVersion = configurations
        .Select(configuration => new AssemblyName(configuration.GetType().Assembly.FullName).Version.ToString())
        .Distinct();

      //var parts = assemblyVersion.Concat(new[] { basePath });
      return string.Join("|", assemblyVersion);
    }

    public static CassetteApplication CassetteApplication { get; private set; }
    
    private static ILogger logger;
  }
}