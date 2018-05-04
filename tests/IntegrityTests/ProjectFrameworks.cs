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
            new TestRunner("*.csproj", "Project files should not be multi-targeted with <TargetFrameworks> element")
                .IgnoreSnippets()
                .Run(projectFilePath =>
                {
                    var xdoc = XDocument.Load(projectFilePath);
                    if (xdoc.Root.Attribute("xmlns") != null)
                    {
                        return true;
                    }

                    var firstTargetFrameworksElement = xdoc.XPathSelectElement("/Project/PropertyGroup/TargetFrameworks");
                    return firstTargetFrameworksElement == null;
                });
        }

        [Test]
        public void EnsureSingleTargetFramework()
        {
            new TestRunner("*.csproj", "Project files should only contain a single <TargetFramework> element")
                .Run(projectFilePath =>
                {
                    var xdoc = XDocument.Load(projectFilePath);
                    if (xdoc.Root.Attribute("xmlns") != null)
                    {
                        return true;
                    }

                    var targetFrameworkElements = xdoc.XPathSelectElements("/Project/PropertyGroup/TargetFramework");

                    // Project may have zero of these if it has TargetFrameworks, but then it fails DoNotUseTargetFrameworksPlural
                    // Projects with no target framework will not build at all
                    return targetFrameworkElements.Count() <= 1;
                });
        }
    }
}
