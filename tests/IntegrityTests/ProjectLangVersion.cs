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

            new TestRunner("*.csproj", "SDK-style project files (VS2017+) must specify LangVersion=7.3 - Provides the best balance between users between samples for netcoreapp2.1/netcoreapp3.1 and samples built by the user likely without LangVersion element.")
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

                    return value == "7.3";
                });
        }
    }
}
