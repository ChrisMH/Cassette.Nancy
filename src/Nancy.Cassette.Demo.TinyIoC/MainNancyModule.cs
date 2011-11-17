namespace Nancy.Cassette.Demo.TinyIoC
{
  public class MainNancyModule : NancyModule
  {
    public MainNancyModule()
    {
      Get["/"] = _ => View["Home"];
    }
  }
}