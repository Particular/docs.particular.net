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
            new TestRunner("*.csproj", "Project files should not be multi-targeted with <TargetFraemworks> element")
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
        public void WillPass()
        {
            new TestRunner("*.csproj", "Should pass")
                .Run(path => true);
        }

        [Test]
        public void WillFail()
        {
            new TestRunner("*.csproj", "Should pass")
                .Run(path => false);
        }
    }
}
