using System;
using System.Collections.Generic;
using System.Linq;
using Cassette.TinyIoC;
using Nancy;

namespace Cassette.Nancy
{
  class NancyContextLifetimeProvider : RequestLifetimeProviderBase
  {
    public NancyContextLifetimeProvider(Func<NancyContext> getContext)
    {
      this.getContext = getContext;
    }

    public override object GetObject()
    {
      try
      { 
        return getContext().Items[keyName];
      }
      catch(KeyNotFoundException)
      {
        return null;
      }
    }

    public override void SetObject(object value)
    {
      getContext().Items[keyName] = value;
    }

    public override void ReleaseObject()
    {
      if (getContext().Items.ContainsKey(keyName))
        base.ReleaseObject();
    }

    private readonly Func<NancyContext> getContext;
    private readonly string keyName = string.Concat("TinyIoC.NancyContext.", Guid.NewGuid());
  }
}
