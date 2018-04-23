using System.Linq;
using System.Xml.XPath;
using NUnit.Framework;

namespace IntegrityTests
{
    public class ProjectFrameworks
    {
       [Test]
        public void DoNotUseTargetFrameworksPlural()
        {
            new TestRunner("*.csproj")
                .Run("Project files should not be multi-targeted with <TargetFraemworks> element", xdoc =>
                {
                    if (xdoc.Root.Attribute("xmlns") != null)
                    {
                        return true;
                    }

                    var firstTargetFrameworksElement = xdoc.XPathSelectElement("/Project/PropertyGroup/TargetFrameworks");
                    return firstTargetFrameworksElement == null;
                });
        }
    }
}
