using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cassette;
using Cassette.IO;
using Cassette.UI;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using TinyIoC;

namespace Nancy.Cassette
{
  public static class Hooks
  {
    // TODO: Optimized output

    public static void Install(NancyConventions conventions, TinyIoCContainer container, IPipelines pipelines)
    {
      // Cassette is used to serve all static content.  Probably.
      conventions.StaticContentsConventions.Clear();


      //storage = IsolatedStorageFile.GetMachineStoreForAssembly();


      configurations = container.ResolveAll<ICassetteConfiguration>();
      rootDirectory = container.Resolve<IRootPathProvider>().GetRootPath();

      //applicationContainer = ShouldOptimizeOutput() ? new CassetteApplicationContainer<CassetteApplication>(CreateCassetteApplication) 
      //                                              : new CassetteApplicationContainer<CassetteApplication>(CreateCassetteApplication, HttpRuntime.AppDomainAppPath);
      pipelines.BeforeRequest.AddItemToStartOfPipeline(OnBeforeRequest);
      pipelines.AfterRequest.AddItemToEndOfPipeline(OnAfterRequest);
    }

    private static Response OnBeforeRequest(NancyContext context)
    {
      Context = context;

      application = new CassetteApplication(
        configurations,
        new FileSystemDirectory(rootDirectory),
        null,
        new UrlGenerator(),
        false,
        GetConfigurationVersion(context.Request.Url.BasePath));

      Assets.GetApplication = () => CassetteApplication;

      return application.OnBeforeRequest(context);
    }

    private static void OnAfterRequest(NancyContext context)
    {
      application.OnAfterRequest(context);
    }

    private static string GetConfigurationVersion(string virtualDirectory)
    {
      var assemblyVersion = configurations.Select(
        configuration => new AssemblyName(configuration.GetType().Assembly.FullName).Version.ToString()
        ).Distinct();

      var parts = assemblyVersion.Concat(new[] {virtualDirectory});
      return string.Join("|", parts);
    }

    public static CassetteApplication CassetteApplication
    {
      get { return application; }
    }

    internal static NancyContext Context { get; private set; }

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