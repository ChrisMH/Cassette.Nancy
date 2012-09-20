using System;
using System.Linq;
using System.Threading;
using Nancy;
using Nancy.Bootstrapper;

namespace Cassette.Nancy
{
  public class CassetteNancyStartup : IApplicationStartup
  {
    private readonly IRootPathProvider rootPathProvider;
    private readonly ThreadLocal<NancyContext> currentContext = new ThreadLocal<NancyContext>(() => null);

    private WebHost webHost;
    
    public CassetteNancyStartup(IRootPathProvider rootPathProvider)
    {
      this.rootPathProvider = rootPathProvider;
      AppDomainAssemblyTypeScanner.LoadAssemblies("Cassette.CoffeeScript.dll");
      AppDomainAssemblyTypeScanner.LoadAssemblies("Cassette.Hogan.dll");
      AppDomainAssemblyTypeScanner.LoadAssemblies("Cassette.JQueryTmpl.dll");
      AppDomainAssemblyTypeScanner.LoadAssemblies("Cassette.KnockoutJQueryTmpl.dll");
      AppDomainAssemblyTypeScanner.LoadAssemblies("Cassette.Less.dll");
      AppDomainAssemblyTypeScanner.LoadAssemblies("Cassette.Sass.dll");
    }
    
    public void Initialize(IPipelines pipelines)
    {
      pipelines.BeforeRequest.AddItemToStartOfPipeline(RunCassetteRequestHandler);

      pipelines.AfterRequest.AddItemToEndOfPipeline(RewriteResponseContents);
    }

    public static bool OptimizeOutput { get; set; }

    private Response RunCassetteRequestHandler(NancyContext context)
    {
      currentContext.Value = context;

      // Some parts of WebHost initialization require a valid NancyContext, so defer
      // creation of the WebHost until the first request hits the pipeline.
      if(webHost == null)
      {
        webHost = new WebHost(rootPathProvider, () => currentContext.Value);
        webHost.Initialize();
      }

      return webHost.RunCassetteRequestHandler(context);
    }

    private void RewriteResponseContents(NancyContext context)
    {
      webHost.RewriteResponseContents(context);
    }

  }
}