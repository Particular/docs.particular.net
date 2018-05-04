using System.Linq;
using System.Xml;
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
                    var nsMgr = new XmlNamespaceManager(new NameTable());
                    var query = "/Project/ItemGroup/PackageReference[@Version='*']";
                    var xmlnsAtt = xdoc.Root.Attribute("xmlns");

                    if (xmlnsAtt != null)
                    {
                        query = "/x:Project/x:ItemGroup/x:PackageReference[@Version='*']";
                        var xmlns = xmlnsAtt.Value;
                        nsMgr.AddNamespace("x", xmlns);
                    }

                    var badPackageRefs = xdoc.XPathSelectElements(query, nsMgr);
                    return !badPackageRefs.Any();
                });
        }
    }
}
