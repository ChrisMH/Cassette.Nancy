using NUnit.Framework;
using Utility.Logging;
using Utility.Logging.NLog;

namespace Nancy.Cassette.Test
{
  [SetUpFixture]
  public static class GlobalTest
  {
    public static ILogger Logger { get; private set; }
    
    [SetUp]
    public static void SetUp()
    {
      Logger = new NLogLoggerFactory().GetCurrentClassLogger();
      CassetteStartup.Logger = Logger;
    }
  }
}