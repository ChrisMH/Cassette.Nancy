using System.Collections.Generic;

namespace Nancy.Cassette
{
  public class CassetteResponseFormatter : IResponseFormatter
  {
    public IEnumerable<ISerializer> Serializers
    {
      get { throw new System.NotImplementedException(); }
    }

    public NancyContext Context
    {
      get { throw new System.NotImplementedException(); }
    }

    public string RootPath
    {
      get { throw new System.NotImplementedException(); }
    }
  }
}