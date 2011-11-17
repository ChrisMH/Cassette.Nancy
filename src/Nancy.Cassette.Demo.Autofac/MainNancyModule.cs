namespace Nancy.Cassette.Demo.Autofac
{
  public class MainNancyModule : NancyModule
  {
    public MainNancyModule()
    {
      Get["/"] = _ => View["Home"];
    }
  }
}