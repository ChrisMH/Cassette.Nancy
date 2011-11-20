using System.Collections.Generic;
using Nancy.ViewEngines.Razor;

namespace Cassette.Nancy.Test
{
  public class TestRazorConfiguration : IRazorConfiguration 
  {
    public bool AutoIncludeModelNamespace
    {
      get { return true; }
    }

    public IEnumerable<string> GetAssemblyNames()
    {
      return new List<string> {"Cassette.Views"};
    }

    public IEnumerable<string> GetDefaultNamespaces()
    {
      return new List<string> {"Cassette.Views"};
    }
  }
}