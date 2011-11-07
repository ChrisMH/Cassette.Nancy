using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using Cassette;
using Cassette.IO;
using Cassette.UI;
using Nancy.Bootstrapper;
using TinyIoC;
using Utility.Logging;

namespace Nancy.Cassette
{
  public class CassetteStartup : IStartup
  {
    public CassetteStartup(TinyIoCContainer container)
    {
      this.container = container;
    }

    public IEnumerable<TypeRegistration> TypeRegistrations
    {
      get { return null; }
    }

    public IEnumerable<CollectionTypeRegistration> CollectionTypeRegistrations
    {
      get { return null; }
    }

    public IEnumerable<InstanceRegistration> InstanceRegistrations
    {
      get { return null; }
    }

    public void Initialize(IPipelines pipelines)
    {
      var configurations = container.ResolveAll<ICassetteConfiguration>().ToList();
      var rootDirectory = container.Resolve<IRootPathProvider>().GetRootPath();
      var cache = new IsolatedStorageDirectory(IsolatedStorageFile.GetMachineStoreForAssembly());

      Func<CassetteApplication> createApplication =
        () => new CassetteApplication(
                configurations,
                new FileSystemDirectory(rootDirectory),
                cache,
                new UrlAndPathGenerator(),
                ShouldOptimizeOutput,
                GetConfigurationVersion(configurations),
                rootDirectory,
                logger);

      applicationContainer = ShouldOptimizeOutput ? new CassetteApplicationContainer<CassetteApplication>(createApplication)
                               : new CassetteApplicationContainer<CassetteApplication>(createApplication, rootDirectory);

      Assets.GetApplication = () => applicationContainer.Application;

      pipelines.BeforeRequest.AddItemToStartOfPipeline(RunCassetteHandlers);
      pipelines.BeforeRequest.AddItemToStartOfPipeline(InitializePlaceholderTracker);
      pipelines.AfterRequest.AddItemToEndOfPipeline(RewriteResponseContents);
    }

    private static string GetConfigurationVersion(IEnumerable<ICassetteConfiguration> configurations)
    {
      var assemblyVersion = configurations
        .Select(configuration => new AssemblyName(configuration.GetType().Assembly.FullName).Version.ToString())
        .Distinct();

      //var parts = assemblyVersion.Concat(new[] { basePath });
      return string.Join("|", assemblyVersion);
    }

    private Response InitializePlaceholderTracker(NancyContext context)
    {
      return applicationContainer.Application.InitializePlaceholderTracker(context);
    }


    private Response RunCassetteHandlers(NancyContext context)
    {
      return applicationContainer.Application.RunCassetteHandlers(context);
    }

    private void RewriteResponseContents(NancyContext context)
    {
      applicationContainer.Application.RewriteResponseContents(context);
    }

    public static bool ShouldOptimizeOutput { get; set; }

    public static ILogger Logger
    {
      set { logger = value.GetCurrentClassLogger(); }
    }

    private readonly TinyIoCContainer container;
    private CassetteApplicationContainer<CassetteApplication> applicationContainer;

    private static ILogger logger;
  }
}