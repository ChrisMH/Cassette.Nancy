using System;
using System.Collections.Generic;
using Cassette.Configuration;
using Nancy;

namespace Cassette.Nancy
{
  internal class CassetteApplication : CassetteApplicationBase
  {
    public const string PlaceholderTrackerContextKey = "CassettePlaceholderTracker";
    public const string ReferenceBuilderContextKey = "CassetteReferenceBuilder";

    public CassetteApplication(IEnumerable<Bundle> bundles, CassetteSettings settings, string cacheVersion,
                               CassetteRouteHandling routeHandling, Func<NancyContext> getCurrentContext)
      : base(bundles, settings, cacheVersion)
    {
      if (getCurrentContext == null) throw new ArgumentNullException("getCurrentContext");
      this.getCurrentContext = getCurrentContext;

      routeHandling.InstallCassetteRouteHandlers(BundleContainer);
    }

    protected override IReferenceBuilder GetOrCreateReferenceBuilder(Func<IReferenceBuilder> create)
    {
      if (getCurrentContext() == null) throw new NullReferenceException("CassetteApplication.GetOrCreateReferenceBuilder : NancyContext has not been set");

      if (getCurrentContext().Items.ContainsKey(ReferenceBuilderContextKey))
      {
        return (IReferenceBuilder) getCurrentContext().Items[ReferenceBuilderContextKey];
      }

      var referenceBuilder = create();
      getCurrentContext().Items[ReferenceBuilderContextKey] = referenceBuilder;
      return referenceBuilder;
    }

    protected override IPlaceholderTracker GetPlaceholderTracker()
    {
      if (getCurrentContext() == null) throw new ApplicationException("CassetteApplication.GetPlaceholderTracker : NancyContext has not been set");
      if (!getCurrentContext().Items.ContainsKey(PlaceholderTrackerContextKey)) throw new ApplicationException("CassetteApplication.GetPlaceholderTracker : PlaceholderTracker has not been initialized");

      return (IPlaceholderTracker)getCurrentContext().Items[PlaceholderTrackerContextKey];
    }

    
    internal Response InitPlaceholderTracker(NancyContext context)
    {
      getCurrentContext().Items[PlaceholderTrackerContextKey] = CreatePlaceholderTracker();
      return null;
    }


    private readonly Func<NancyContext> getCurrentContext;

  }
}