using System;
using System.Collections.Generic;
using Cassette;
using Cassette.IO;
using Cassette.UI;

namespace Nancy.Cassette
{
  public class CassetteApplication : CassetteApplicationBase
  {
    static readonly string PlaceholderTrackerKey = typeof(IPlaceholderTracker).FullName;

    public CassetteApplication(IEnumerable<ICassetteConfiguration> configurations,
                               IDirectory rootDirectory, IDirectory cacheDirectory, IUrlGenerator urlGenerator,
                               bool isOutputOptimized, string version)
      : base(configurations, rootDirectory, cacheDirectory, urlGenerator, isOutputOptimized, version)
    {
    }

    public Response OnBeforeRequest(NancyContext context)
    {
      Console.WriteLine("CassetteApplication.OnBeforeRequest : {0}", context.Request.Url.Path);

      this.context = context;

      IPlaceholderTracker tracker;
      if (HtmlRewritingEnabled)
      {
          tracker = new PlaceholderTracker();
      }
      else
      {
          tracker = new NullPlaceholderTracker();
      }
      this.context.Items[PlaceholderTrackerKey] = tracker;

      return null;
    }
    
    public void OnAfterRequest(NancyContext context)
    {
      Console.WriteLine("CassetteApplication.OnAfterRequest : {0}", context.Request.Url.Path);
      
    }


    protected override IReferenceBuilder<T> GetOrCreateReferenceBuilder<T>(Func<IReferenceBuilder<T>> create)
    {
      var key = "ReferenceBuilder:" + typeof (T).FullName;
      if (context.Items.ContainsKey(key))
      {
        return (IReferenceBuilder<T>) context.Items[key];
      }

      var builder = create();
      context.Items[key] = builder;
      return builder;
    }

    protected override IPlaceholderTracker GetPlaceholderTracker()
    {
      return (IPlaceholderTracker)context.Items[PlaceholderTrackerKey];
    }

    private NancyContext context;
  }
}