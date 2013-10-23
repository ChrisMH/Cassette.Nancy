using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using Nancy.Bootstrapper;

namespace Cassette.Nancy {
    public class CassetteNancyPluginRegistrations : ApplicationRegistrations {
        public CassetteNancyPluginRegistrations() {
            AppDomainAssemblyTypeScanner.AddAssembliesToScan(new string[] { 
                  "Cassette.CoffeeScript.dll"
                , "Cassette.Hogan.dll"
                , "Cassette.JQueryTmpl.dll"
                , "Cassette.KnockoutJQueryTmpl.dll"
                , "Cassette.Less.dll"
                , "Cassette.Sass.dll"
            });
        }
    }
}
