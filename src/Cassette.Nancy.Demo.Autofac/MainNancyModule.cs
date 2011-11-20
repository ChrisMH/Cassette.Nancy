using Nancy;

namespace Cassette.Nancy.Demo.Autofac
{
  public class MainNancyModule : NancyModule
  {
    public MainNancyModule()
    {
      Get["/"] = _ => View["Home"];
    }
  }
}