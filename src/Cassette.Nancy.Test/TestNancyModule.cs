using Nancy;

namespace Cassette.Nancy.Test
{
  public class TestNancyModule : NancyModule
  {
    public TestNancyModule()
    {
      Get["/"] = _ => "Empty";
      Get["/RazorHome"] = _ => View["RazorHome.cshtml"];
    }
  }
}
