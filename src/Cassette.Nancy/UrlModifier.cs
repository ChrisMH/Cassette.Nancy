using System;
using System.Linq;
using Nancy;
using Nancy.Extensions;

namespace Cassette.Nancy
{
  class UrlModifier : IUrlModifier
  {
    public UrlModifier(Func<NancyContext> getContext)
    {
      this.getContext = getContext;
    }

    public string Modify(string url)
    {
      return getContext().ToFullPath(string.Concat("~/", url));
    }

    private readonly Func<NancyContext> getContext;
  }
}
