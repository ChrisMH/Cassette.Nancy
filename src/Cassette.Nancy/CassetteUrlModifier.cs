using System;
using System.Linq;

namespace Cassette.Nancy
{
  class CassetteUrlModifier : IUrlModifier
  {
    public string Modify(string url)
    {
      return url;
    }
  }
}
