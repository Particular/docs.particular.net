using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using NUnit.Framework;

namespace IntegrityTests
{
    public class ProjectFrameworks
    {
        [Test]
        public void DoNotUseTargetFrameworksPlural()
        {
            var currentDirectory = TestContext.CurrentContext.TestDirectory;
            var docsDirectory = Path.GetFullPath(Path.Combine(currentDirectory, @"..\..\..\..\.."));

            var projectFiles = Directory.GetFiles(docsDirectory, "*.csproj", SearchOption.AllDirectories);

            var badProjects = new List<string>();

            foreach (var projectFilePath in projectFiles)
            {
                var filename = Path.GetFileName(projectFilePath);
                bool isCore = filename.EndsWith(".Core.csproj");

                var xdoc = XDocument.Load(projectFilePath);

                var xmlns = xdoc.Root.Attribute("xmlns");
                if (xmlns != null)
                {
                    continue;
                }
                
                var firstTargetFrameworksElement = xdoc.XPathSelectElement("/Project/PropertyGroup/TargetFrameworks");

                if (firstTargetFrameworksElement != null)
                {
                    badProjects.Add(projectFilePath);
                }
            }

            Assert.IsEmpty(badProjects, "Project files should not be multi-targeted with <TargetFraemworks> element.");
        }


    }

}
