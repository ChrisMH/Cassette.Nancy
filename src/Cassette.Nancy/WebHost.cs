using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cassette.Nancy
{
  public class WebHost : HostBase
  {
    protected override IEnumerable<Assembly> LoadAssemblies()
    {
      // TODO: Implement this method
      throw new NotImplementedException();
    }

    protected override IConfiguration<CassetteSettings> CreateHostSpecificSettingsConfiguration()
    {
      // TODO: Implement this method
      throw new NotImplementedException();
    }

    protected override bool CanCreateRequestLifetimeProvider
    {
      get
      {
        // TODO: Implement this property getter
        throw new NotImplementedException();
      }
    }
  }
}
