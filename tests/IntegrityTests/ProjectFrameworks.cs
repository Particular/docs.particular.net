using System;
using System.Xml.Linq;
using System.Xml.XPath;
using NUnit.Framework;

namespace IntegrityTests
{
    public class ProjectFrameworks
    {
        [Test]
        public void TargetFrameworkElementShouldAgreeWithFrameworkCount()
        {
            new TestRunner("*.csproj", "Project files with <TargetFrameworks> element should list multiple frameworks")
                .IgnoreSnippets()
                .Run(projectFilePath =>
                {
                    var xdoc = XDocument.Load(projectFilePath);
                    if (xdoc.Root.Attribute("xmlns") != null)
                    {
                        return true;
                    }

                    var firstTargetFrameworksElement = xdoc.XPathSelectElement("/Project/PropertyGroup/TargetFrameworks");
                    if(firstTargetFrameworksElement == null)
                    {
                        return true;
                    }

                    var tfmList = firstTargetFrameworksElement.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    return tfmList.Length > 1;
                });
        }
    }
}
