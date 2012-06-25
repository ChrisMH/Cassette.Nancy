namespace Cassette.Nancy
{
    public class HandlerPathPrepender : IUrlModifier
    {
        public string Modify(string url)
        {
            return "cassette.axd/" + url;
        }
    }
}