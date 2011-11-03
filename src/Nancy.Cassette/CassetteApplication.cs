using System;
using System.Collections.Generic;
using System.IO;
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
      IPlaceholderTracker tracker;
      if (HtmlRewritingEnabled)
      {
          tracker = new PlaceholderTracker();
      }
      else
      {
          tracker = new NullPlaceholderTracker();
      }
      context.Items[PlaceholderTrackerKey] = tracker;

      return null;
    }
    
    public void OnAfterRequest(NancyContext context)
    {
      var currentContents = context.Response.Contents;
      context.Response.Contents =
        stream =>
        {
          var currentContentsStream = new MemoryStream();
          currentContents(currentContentsStream);
          currentContentsStream.Position = 0;
          
          var reader = new StreamReader(currentContentsStream);

          var html = reader.ReadToEnd();

          html = GetPlaceholderTracker().ReplacePlaceholders(html);

          var writer = new StreamWriter(stream);
          writer.Write(html);

          writer.Flush();
        };
    }
    
    protected override IReferenceBuilder<T> GetOrCreateReferenceBuilder<T>(Func<IReferenceBuilder<T>> create)
    {
      var key = "ReferenceBuilder:" + typeof (T).FullName;
      if (Hooks.Context.Items.ContainsKey(key))
      {
        return (IReferenceBuilder<T>) Hooks.Context.Items[key];
      }

      var builder = create();
      Hooks.Context.Items[key] = builder;
      return builder;
    }

    protected override IPlaceholderTracker GetPlaceholderTracker()
    {
      return (IPlaceholderTracker)Hooks.Context.Items[PlaceholderTrackerKey];
    }
  }
}