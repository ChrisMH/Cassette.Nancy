﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
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

      var cache = new IsolatedStorageDirectory(IsolatedStorageFile.GetMachineStoreForAssembly());

      /*
      var cassetteDirectory = Path.Combine(rootDirectory, "_cassette");
      if(!Directory.Exists(cassetteDirectory))
      {
        Directory.CreateDirectory(cassetteDirectory);
      }
      var cache = new FileSystemDirectory(cassetteDirectory);
      */

      //applicationContainer = ShouldOptimizeOutput() ? new CassetteApplicationContainer<CassetteApplication>(CreateCassetteApplication) 
      //                                              : new CassetteApplicationContainer<CassetteApplication>(CreateCassetteApplication, HttpRuntime.AppDomainAppPath);



      CassetteApplication = new CassetteApplication(
        configurations,
        new FileSystemDirectory(rootDirectory),
        cache, 
        new UrlGenerator(),
        ShouldOptimizeOutput,
        GetConfigurationVersion(configurations),
        conventions, pipelines,
        logger);

      Assets.GetApplication = () => CassetteApplication;
    }
    
    public static bool ShouldOptimizeOutput
    {
      get { return shouldOptimizeOutput ?? (bool)(shouldOptimizeOutput = !GetDebugMode()); }
      set { shouldOptimizeOutput = value; }
    }


    private static string GetConfigurationVersion(IEnumerable<ICassetteConfiguration> configurations)
    {
      var assemblyVersion = configurations
        .Select(configuration => new AssemblyName(configuration.GetType().Assembly.FullName).Version.ToString())
        .Distinct();

      //var parts = assemblyVersion.Concat(new[] { basePath });
      return string.Join("|", assemblyVersion);
    }

    private static bool GetDebugMode()
    {
      try
      {
        var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(DebuggableAttribute), true);
        if (attributes.Length == 0)
        {
          return false;
        }

        var debuggable = (DebuggableAttribute)attributes[0];
        return debuggable.IsJITTrackingEnabled;
      }
      catch (Exception)
      {
        return false;
      }
    }


    public static CassetteApplication CassetteApplication { get; private set; }
    
    private static bool? shouldOptimizeOutput;
    private static ILogger logger;
  }
}