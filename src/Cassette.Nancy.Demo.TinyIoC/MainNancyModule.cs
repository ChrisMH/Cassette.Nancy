namespace Cassette.Nancy.Demo.TinyIoC
{
  public class MainNancyModule : NancyModule
  {
    public MainNancyModule()
    {
      Get["/"] = _ => View["Home"];
    }
  }
}