using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nancy;
using Nancy.Bootstrapper;
using Utility.Logging;

namespace Cassette.Nancy
{
  public class WebHost : HostBase
  {
    public WebHost(IRootPathProvider rootPathProvider, Func<NancyContext> getContext, ILoggerFactory loggerFactory)
    {
      this.rootPathProvider = rootPathProvider;
      this.getContext = getContext;
      this.logger = loggerFactory.GetCurrentInstanceLogger();
    }

    public Response RunCassetteRequestHandler(NancyContext context)
    {
      return null;
    }

    public void RewriteResponseContents(NancyContext context)
    {
      /*
      if (!context.Items.ContainsKey(CassetteApplication.PlaceholderTrackerContextKey))
      {
      // InitializePlaceholderTracker was not called for this request.  Do not attempt to rewrite.
      return;
      }
      if (!context.Response.ContentType.Equals("text/html"))
      {
      // Only html needs to be (possibly) rewritten
      return;
      }
      var currentContents = context.Response.Contents;
      context.Response.Contents =
      stream =>
      {
      //if (Logger != null)
      //  Logger.Trace("RewriteResponseContents : {0} : {1} : content type = {2}", Thread.CurrentThread.ManagedThreadId, context.Request.Url.Path,
      //    context.Response.ContentType);
      var currentContentsStream = new MemoryStream();
      currentContents(currentContentsStream);
      var reader = new StreamReader(currentContentsStream);
      //if (Logger != null)
      //{
      //  reader.BaseStream.Seek(0, SeekOrigin.Begin);
      //  Logger.Trace("RewriteResponseContents : {0} : Original Contents: \r\n {1}", Thread.CurrentThread.ManagedThreadId, reader.ReadToEnd());
      //}
      reader.BaseStream.Seek(0, SeekOrigin.Begin);
      var writer = new StreamWriter(stream);
      writer.Write(((IPlaceholderTracker)context.Items[CassetteApplication.PlaceholderTrackerContextKey]).ReplacePlaceholders(reader.ReadToEnd()));
      writer.Flush();
      //if (Logger != null)
      //{
      //  var outputReader = new StreamReader(stream);
      //  outputReader.BaseStream.Seek(0, SeekOrigin.Begin);
      //  Logger.Trace("RewriteResponseContents : {0} : Rewritten Contents: \r\n {1}", Thread.CurrentThread.ManagedThreadId, outputReader.ReadToEnd());
      //}
      };
      */
    }
     
    protected override IEnumerable<Assembly> LoadAssemblies()
    {
      return AppDomainAssemblyTypeScanner.Assemblies;
    }

    protected override IConfiguration<CassetteSettings> CreateHostSpecificSettingsConfiguration()
    {
      return new WebHostSettingsConfiguration(rootPathProvider);
    }

    protected override bool CanCreateRequestLifetimeProvider
    {
      get { return true; }
    }

    protected override TinyIoC.TinyIoCContainer.ITinyIoCObjectLifetimeProvider CreateRequestLifetimeProvider()
    {
      return new NancyContextLifetimeProvider(getContext);
    }

    protected override void ConfigureContainer()
    {
      Container.Register<IUrlModifier>((c, p) => new UrlModifier());
      base.ConfigureContainer();
    }
    
    private readonly IRootPathProvider rootPathProvider;
    private readonly Func<NancyContext> getContext;
    private readonly ILogger logger;
  }
}