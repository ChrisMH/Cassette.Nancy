using System;
using System.Linq;

namespace Cassette.Nancy
{
  class UrlModifier : IUrlModifier
  {
    public string Modify(string url)
    {
      return url;
    }
  }
}
