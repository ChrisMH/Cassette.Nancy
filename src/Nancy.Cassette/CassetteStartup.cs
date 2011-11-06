using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using Cassette;
using Cassette.IO;
using Cassette.UI;
using Nancy.Bootstrapper;
using TinyIoC;

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
      get { return new List<InstanceRegistration> { new InstanceRegistration(typeof (CassetteApplication), application) }; }
    }

    public void Initialize(IPipelines pipelines)
    {
      var configurations = container.ResolveAll<ICassetteConfiguration>().ToList();
      var rootDirectory = container.Resolve<IRootPathProvider>().GetRootPath();

      var cache = new IsolatedStorageDirectory(IsolatedStorageFile.GetMachineStoreForAssembly());

      /*
      var cassetteDirectory = Path.Combine(rootDirectory, "_cassette");
      if(!Directory.Exists(cassetteDirectory))
      {
        Directory.CreateDirectory(cassetteDirectory);
      }
      var cache = new FileSystemDirectory(cassetteDirectory);

      //applicationContainer = ShouldOptimizeOutput() ? new CassetteApplicationContainer<CassetteApplication>(CreateCassetteApplication) 
      //                                              : new CassetteApplicationContainer<CassetteApplication>(CreateCassetteApplication, HttpRuntime.AppDomainAppPath);

      */
      application = new CassetteApplication(
        configurations,
        new FileSystemDirectory(rootDirectory),
        cache,
        new UrlAndPathGenerator(),
        ShouldOptimizeOutput,
        GetConfigurationVersion(configurations));

      Assets.GetApplication = () => application;

      Trace.Source.TraceInformation("CassetteStartup.Initialize");
    }

    private static string GetConfigurationVersion(IEnumerable<ICassetteConfiguration> configurations)
    {
      var assemblyVersion = configurations
        .Select(configuration => new AssemblyName(configuration.GetType().Assembly.FullName).Version.ToString())
        .Distinct();

      //var parts = assemblyVersion.Concat(new[] { basePath });
      return string.Join("|", assemblyVersion);
    }


    public static bool ShouldOptimizeOutput
    {
      get { return shouldOptimizeOutput ?? (bool) (shouldOptimizeOutput = !GetDebugMode()); }
      set { shouldOptimizeOutput = value; }
    }

    private static bool GetDebugMode()
    {
      try
      {
        var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (DebuggableAttribute), true);
        if (attributes.Length == 0)
        {
          return false;
        }

        var debuggable = (DebuggableAttribute) attributes[0];
        return debuggable.IsJITTrackingEnabled;
      }
      catch (Exception)
      {
        return false;
      }
    }

    private readonly TinyIoCContainer container;
    private CassetteApplication application;

    private static bool? shouldOptimizeOutput;
  }
}