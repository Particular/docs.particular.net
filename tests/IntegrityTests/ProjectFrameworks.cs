using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
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
                    if (firstTargetFrameworksElement == null)
                    {
                        return true;
                    }

                    var tfmList = firstTargetFrameworksElement.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    return tfmList.Length > 1;
                });
        }

        static readonly string[] sdkProjectAllowedTfmList = new[] { "net5.0", "netcoreapp3.1", "netcoreapp2.1", "net48", "netstandard2.0" };
        static readonly string[] nonSdkProjectAllowedFrameworkList = new[] { "v4.8" };

        [Test]
        public void RestrictTargetFrameworks()
        {
            var tfmString = string.Join(", ", sdkProjectAllowedTfmList);

            new TestRunner("*.csproj", "Allowed target frameworks are: " + tfmString)
                .Run(projectFilePath =>
                {
                    var xdoc = XDocument.Load(projectFilePath);
                    var xmlnsNode = xdoc.Root.Attribute("xmlns");
                    if (xmlnsNode != null)
                    {
                        var mgr = new XmlNamespaceManager(new NameTable());
                        mgr.AddNamespace("x", xmlnsNode.Value);

                        foreach(var node in xdoc.XPathSelectElements("/x:Project/x:PropertyGroup/x:TargetFramework", mgr))
                        {
                            if(!nonSdkProjectAllowedFrameworkList.Contains(node.Value))
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
                            foreach(var tfm in node.Value.Split(';'))
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
    }
}
