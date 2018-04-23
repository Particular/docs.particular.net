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

        public TestRunner(string glob)
        {
            this.glob = glob;
        }
        
        public void Run(string message, Func<XDocument, bool> testDelegate)
        {
            var badProjects = new List<string>();

            foreach (var rootPath in TestSetup.RootDirectories)
            {
                var projectFiles = Directory.GetFiles(rootPath, glob, SearchOption.AllDirectories);

                foreach (var projectFilePath in projectFiles)
                {
                    var xdoc = XDocument.Load(projectFilePath);

                    bool success = testDelegate(xdoc);

                    if (!success)
                    {
                        badProjects.Add(projectFilePath);
                    }
                }
            }

            if (badProjects.Count > 0)
            {
                Assert.Fail($"{message}:\r\n  > {string.Join("\r\n  > ", badProjects)}");
            }
        }




    }
}