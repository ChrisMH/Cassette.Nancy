using System;
using System.Linq;
using Nancy;
using Utility.Logging;

namespace Cassette.Nancy
{
  class CassetteRequestHandler
  {

    public CassetteRequestHandler(ILoggerFactory loggerFactory)
    {
      this.loggerFactory = loggerFactory;
      this.logger = loggerFactory.GetCurrentInstanceLogger();
    }

    public Response Run(NancyContext context)
    {
      logger.Info("Run");
      return null;
    }

    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger logger;
  }
}
