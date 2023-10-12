using System;
using System.IO;
using NUnit.Framework;

namespace IntegrityTests
{
    public class OnlyOneNugetConfig
    {
        [Test]
        public void VerifyProjectsDontHaveExtraNugetConfigs()
        {
            var rootPath = Path.Combine(TestSetup.DocsRootPath, "nuget.config");

            new TestRunner("nuget.config", "Projects in this repo should not have their own nuget.config entries, they use the one at the repo root. The DocsEngine inserts the nuget.config file into generated zip downloads")
                .Run(path =>
                {
                    return string.Equals(rootPath, path, StringComparison.OrdinalIgnoreCase);
                });
        }
    }
}
