using System;
using System.Linq;
using Nancy;
using Nancy.Bootstrapper;

namespace Cassette.Nancy
{
  public class NancyCassetteStartup : IStartup
  {
    public NancyCassetteStartup(IRootPathProvider rootPathProvider)
    {
    }
  }
}
