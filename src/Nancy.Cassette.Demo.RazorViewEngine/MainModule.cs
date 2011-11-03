namespace Nancy.Cassette.Demo.RazorViewEngine
{
  public class MainModule : NancyModule
  {
    public MainModule()
    {
      Get["/"] = _ => View["Home"];
    }
  }
}