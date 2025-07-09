using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using NUnit.Framework;

namespace IntegrityTests
{
    public class ProjectFrameworks
    {
        static readonly char[] separator = [';'];

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
                    if (firstTargetFrameworksElement == null)
                    {
                        return true;
                    }

                    var tfmList = firstTargetFrameworksElement.Value.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                    return tfmList.Length > 1;
                });
        }

        public static readonly string[] sdkProjectAllowedTfmList = ["net10.0", "net10.0-windows", "net9.0", "net9.0-windows", "net8.0", "net8.0-windows", "net48", "netstandard2.0"];
        static readonly string[] nonSdkProjectAllowedFrameworkList = ["v4.8"];

        [Test]
        public void RestrictTargetFrameworks()
        {
            var tfmString = string.Join(", ", sdkProjectAllowedTfmList);

            new TestRunner("*.csproj", "Allowed target frameworks are: " + tfmString)
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

                    var xmlnsNode = xdoc.Root.Attribute("xmlns");
                    if (xmlnsNode != null)
                    {
                        var mgr = new XmlNamespaceManager(new NameTable());
                        mgr.AddNamespace("x", xmlnsNode.Value);

                        foreach (var node in xdoc.XPathSelectElements("/x:Project/x:PropertyGroup/x:TargetFramework", mgr))
                        {
                            if (!nonSdkProjectAllowedFrameworkList.Contains(node.Value))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        foreach (var node in xdoc.XPathSelectElements("/Project/PropertyGroup/TargetFramework"))
                        {
                            if (!sdkProjectAllowedTfmList.Contains(node.Value))
                            {
                                return false;
                            }
                        }
                        foreach (var node in xdoc.XPathSelectElements("/Project/PropertyGroup/TargetFrameworks"))
                        {
                            foreach (var tfm in node.Value.Split(';'))
                            {
                                if (!sdkProjectAllowedTfmList.Contains(tfm))
                                {
                                    return false;
                                }
                            }
                        }
                    }

                    return true;
                });
        }

        [Test]
        public void SnippetsShouldNotBeMultiTargeted()
        {
            new TestRunner("*.csproj", "Snippets projects should not be multi-targeted")
                .IgnoreSamples()
                .IgnoreTutorials()
                .Run(projectFilePath =>
                {
                    var xdoc = XDocument.Load(projectFilePath);
                    if (xdoc.Root.Attribute("xmlns") != null)
                    {
                        return true;
                    }

                    var firstTargetFrameworksElement = xdoc.XPathSelectElement("/Project/PropertyGroup/TargetFrameworks");

                    return firstTargetFrameworksElement is null;

                });
        }
    }
}
