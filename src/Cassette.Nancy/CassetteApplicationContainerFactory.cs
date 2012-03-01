using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Cassette.Configuration;
using Nancy;

namespace Cassette.Nancy
{
  class CassetteApplicationContainerFactory : CassetteApplicationContainerFactoryBase<CassetteApplication>
  {
    public CassetteApplicationContainerFactory(ICassetteConfigurationFactory cassetteConfigurationFactory,
                                               CassetteConfigurationSection configurationSection,
                                               string physicalDirectory,
                                               string virtualDirectory,
                                               bool isDebuggingEnabled,
                                               Func<NancyContext> getCurrentContext,
                                               CassetteRouteGenerator routeGenerator)
      : base(cassetteConfigurationFactory, configurationSection, physicalDirectory, virtualDirectory)
    {
      this.configurationSection = configurationSection;
      this.physicalDirectory = physicalDirectory;
      this.virtualDirectory = virtualDirectory;
      this.isDebuggingEnabled = isDebuggingEnabled;
      this.getCurrentContext = getCurrentContext;
      this.routeGenerator = routeGenerator;
    }

    public override CassetteApplicationContainer<CassetteApplication> CreateContainer()
    {
      var container = base.CreateContainer();
      container.IgnoreFileSystemChange(
        new Regex(
          "^" + Regex.Escape(Path.Combine(PhysicalApplicationDirectory, "App_Data")),
          RegexOptions.IgnoreCase));
      return container;
    }

    protected override IEnumerable<ICassetteConfiguration> CreateCassetteConfigurations()
    {
      yield return new InitialConfiguration(isDebuggingEnabled, physicalDirectory, routeGenerator);
      foreach (var configuration in base.CreateCassetteConfigurations())
      {
        yield return configuration;
      }
    }
    
    protected override string GetConfigurationVersion()
    {
      var assemblyVersion = CassetteConfigurations
                                .Select(configuration => configuration.GetType().Assembly.FullName)
                                .Distinct()
                                .Select(name => new AssemblyName(name).Version.ToString());

      var parts = assemblyVersion.Concat(new[] { virtualDirectory });
      return string.Join("|", parts);
    }

    protected override CassetteApplication CreateCassetteApplicationCore(IBundleContainer bundleContainer, CassetteSettings settings)
    {
      return new CassetteApplication(bundleContainer,
        settings,
        routeGenerator,
        getCurrentContext);
    }

    protected override bool ShouldWatchFileSystem
    {
      get
      {
        if (configurationSection.WatchFileSystem.HasValue)
        {
          return configurationSection.WatchFileSystem.Value;
        }
        else
        {
          return isDebuggingEnabled;
        }
      }
    }

    protected override string PhysicalApplicationDirectory
    {
      get
      {
        return physicalDirectory;
      }
    }

    private readonly CassetteConfigurationSection configurationSection;
    private readonly string physicalDirectory;
    private readonly string virtualDirectory;
    private readonly bool isDebuggingEnabled;
    private readonly CassetteRouteGenerator routeGenerator;
    private readonly Func<NancyContext> getCurrentContext;
  }
}