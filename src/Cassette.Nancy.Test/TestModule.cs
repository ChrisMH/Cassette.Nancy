using Nancy;

namespace Cassette.Nancy.Test
{
  public class TestModule : NancyModule
  {
    public TestModule()
    {
      Get["/"] = _ => "Empty";
      Get["/RazorHome"] = _ => View["RazorHome.cshtml"];
    }
  }
}
