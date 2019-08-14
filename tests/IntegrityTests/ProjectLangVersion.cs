using System.Xml.Linq;
using System.Xml.XPath;
using NUnit.Framework;

namespace IntegrityTests
{
    public class ProjectLangVersion
    {
       [Test]
        public void ShouldUseLangVersionLatest()
        {
            // Also reflected in https://docs.particular.net/samples/#technology-choices-c-language-level
            // And in /tools/projectStandards.linq

            new TestRunner("*.csproj", "VS2017 format project files must specify LangVersion=7.1 - This is the default in VS2019 but the property is needed to run correctly in VS2017.")
                .IgnoreSnippets()
                .Run(projectFilePath =>
                {
                    var xdoc = XDocument.Load(projectFilePath);

                    // Ignore non-Sdk (VS2015 and previous) style projects
                    var sdk = xdoc.Root.Attribute("Sdk");
                    if (sdk == null)
                    {
                        return true;
                    }

                    var firstTargetFrameworksElement = xdoc.XPathSelectElement("/Project/PropertyGroup/LangVersion");
                    var value = firstTargetFrameworksElement?.Value;

                    return value == "7.1";
                });
        }
    }
}
