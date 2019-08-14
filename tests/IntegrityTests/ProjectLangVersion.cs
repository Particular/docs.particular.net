using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using NUnit.Framework;

namespace IntegrityTests
{
    using System;

    public class ProjectLangVersion
    {
       [Test]
        public void ShouldUseLangVersionLatest()
        {
            new TestRunner("*.csproj", "VS2017 format project files must specify LangVersion=7.1 - This is the default in VS2019 but the property is needed to run correctly in VS2017.")
                .IgnoreSnippets()
                .Run(projectFilePath =>
                {
                    var xdoc = XDocument.Load(projectFilePath);

                    // Ignore VS2015 style projects
                    if (xdoc.Root.Attribute("xmlns") != null)
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
