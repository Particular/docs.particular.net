using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using NUnit.Framework;

namespace IntegrityTests
{
    public class TestRunner
    {
        private string glob;
        private string errorMessage;

        public TestRunner(string glob, string errorMessage)
        {
            this.glob = glob;
            this.errorMessage = errorMessage;
        }

        public void Run(Func<string, bool> testDelegate)
        {
            var badProjects = new List<string>();

            foreach (var rootPath in TestSetup.RootDirectories)
            {
                var projectFiles = Directory.GetFiles(rootPath, glob, SearchOption.AllDirectories);

                foreach (var projectFilePath in projectFiles)
                {
                    bool success = testDelegate(projectFilePath);

                    if (!success)
                    {
                        badProjects.Add(projectFilePath);
                    }
                }
            }

            if (badProjects.Count > 0)
            {
                Assert.Fail($"{errorMessage}:\r\n  > {string.Join("\r\n  > ", badProjects)}");
            }
        }
    }
}