namespace Nancy.Cassette.Demo.SuperSimpleViewEngine
{
  public class MainModule : NancyModule
  {
    public MainModule()
    {
      Get["/"] = _ => View["Home"];
    }
  }
}