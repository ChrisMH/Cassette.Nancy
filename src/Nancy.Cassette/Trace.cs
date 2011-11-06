using System.Diagnostics;

namespace Nancy.Cassette
{
  public class Trace
  {
    public static readonly TraceSource Source = new TraceSource("Nancy.Cassette");
  }
}