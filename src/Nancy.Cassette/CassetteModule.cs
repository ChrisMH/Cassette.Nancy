using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nancy.Cassette
{
  public class CassetteModule : NancyModule
  {
    public CassetteModule(CassetteApplication application)
    {
      application.InstallRoutes(this);
    }

    
  }
}
