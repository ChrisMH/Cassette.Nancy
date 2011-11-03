using System;
using System.Collections.Generic;
using Cassette;
using Cassette.IO;
using Cassette.UI;

namespace Nancy.Cassette
{
  public class CassetteApplication : CassetteApplicationBase 
  {
    public CassetteApplication(IEnumerable<ICassetteConfiguration> configurations, IDirectory rootDirectory, IDirectory cacheDirectory, IUrlGenerator urlGenerator, bool isOutputOptimized, string version) 
    : base(configurations, rootDirectory, cacheDirectory, urlGenerator, isOutputOptimized, version)
    {
    }

    protected override IReferenceBuilder<T> GetOrCreateReferenceBuilder<T>(Func<IReferenceBuilder<T>> create)
    {
      throw new NotImplementedException();
    }

    protected override IPlaceholderTracker GetPlaceholderTracker()
    {
      throw new NotImplementedException();
    }
  }
}