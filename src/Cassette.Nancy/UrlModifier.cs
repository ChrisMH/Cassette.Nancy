using System;
using System.Linq;
using Nancy;
using Nancy.Extensions;

namespace Cassette.Nancy
{
  class UrlModifier : IUrlModifier
  {
    public const string CassettePrefix = "/_cassette";

    public UrlModifier(Func<NancyContext> getContext)
    {
      this.getContext = getContext;
    }

    public string Modify(string url)
    {
      return getContext().ToFullPath(string.Concat("~", CassettePrefix, "/", url));
    }

    private readonly Func<NancyContext> getContext;
  }
}
