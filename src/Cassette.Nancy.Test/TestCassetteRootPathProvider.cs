using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using System.IO; 

namespace Cassette.Nancy.Test {
    public class TestCassetteRootPathProvider : IRootPathProvider {
        private static readonly string RootPath;

        static TestCassetteRootPathProvider() {
            var directoryName = Path.GetDirectoryName(typeof(TestNancyModule).Assembly.CodeBase);

            if (directoryName != null) {
                var assemblyPath = directoryName.Replace(@"file:\", string.Empty);

                RootPath = Path.Combine(assemblyPath, @"..", "..", "..", "Cassette.Nancy.Test");
                Console.WriteLine("RootPath: '{0}'", RootPath);
            }
        }

        public string GetRootPath() {
            return RootPath;
        }
    }
}
