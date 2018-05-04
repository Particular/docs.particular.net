using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using NUnit.Framework;

namespace IntegrityTests
{
    public class ReferenceVersions
    {
       [Test]
        public void MustScopeReferencesToAMajorVersion()
        {
            new TestRunner("*.csproj", "Package References cannot have Version=\"*\" or restore will sometimes fail and yield old, incorrect, or mismatched versions.")
                .Run(projectFilePath =>
                {
                    var xdoc = XDocument.Load(projectFilePath);
                    if (xdoc.Root.Attribute("xmlns") != null)
                    {
                        // Ignore pre-VS2017 projects
                        return true;
                    }

                    var badPackageRefs = xdoc.XPathSelectElements("/Project/ItemGroup/PackageReference[@Version='*']");
                    return !badPackageRefs.Any();
                });
        }
    }
}
