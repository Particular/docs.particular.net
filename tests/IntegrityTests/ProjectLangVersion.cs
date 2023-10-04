using System.Xml.Linq;
using System.Xml.XPath;
using NUnit.Framework;

namespace IntegrityTests
{
    public class ProjectLangVersion
    {
        //TODO-Do we need this?`
        //[Test]
        public void ShouldUseLangVersion10()
        {
            // Also reflected in https://docs.particular.net/samples/#technology-choices-c-language-level
            // And in /tools/projectStandards.linq

            new TestRunner("*.csproj", "SDK-style project files must specify LangVersion=10.0")
                .Run(projectFilePath =>
                {
                    XDocument xdoc = default;

                    try
                    {
                        xdoc = XDocument.Load(projectFilePath);
                    }
                    catch
                    {
                        return false;
                    }

                    // Ignore if not an Sdk-style project
                    var sdk = xdoc.Root.Attribute("Sdk");
                    if (sdk == null)
                    {
                        return true;
                    }

                    var firstLangVersionElement = xdoc.XPathSelectElement("/Project/PropertyGroup/LangVersion");
                    var value = firstLangVersionElement?.Value;

                    return value == "10.0";
                });
        }
    }
}
