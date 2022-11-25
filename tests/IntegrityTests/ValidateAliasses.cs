using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using NuGet.Versioning;
using NUnit.Framework;

namespace IntegrityTests
{
    public class ValidateAliasses
    {
        [Test]
        public void ValidateAliassesVersionMatchesPackageReferenceVersion()
        {

            new TestRunner("*.csproj", "Found alias version not matching package reference version.")
                .Run(projPath =>
                {
                    var (versionedPath, aliasName, aliasVersionSuffix) = GetVersionedAliasPath(projPath);

                    if (versionedPath == null)
                    {
                        return true;
                    }

                    if (aliasVersionSuffix == "All")
                    {
                        return true;
                    }

                    // Temporarily skipping over, this probably means incorrect component names, but that's another test
                    if (!TestSetup.NugetAliases.TryGetValue(aliasName, out var folderPackageId))
                    {
                        return true;
                    }

                    foreach (var packageRef in QueryPackageRefs(projPath))
                    {
                        var packageId = packageRef.Attribute("Include")!.Value;
                        var packageVersion = packageRef.Attribute("Version")!.Value;

                        if (packageId == folderPackageId)
                        {
                            if (aliasVersionSuffix == "1" && packageVersion.StartsWith("0.")) continue; // Valid for previews

                            var packageVersionPrefix = packageVersion;
                            var i = packageVersionPrefix.IndexOf('*');
                            if (i >= 0)
                            {
                                packageVersionPrefix = packageVersionPrefix[..(i - 1)];
                            }

                            if (packageVersionPrefix != aliasVersionSuffix)
                            {
                                throw new Exception($"Suffix: {aliasVersionSuffix}, Version: {packageVersion}");
                            }
                        }
                    }

                    return true;
                });
        }

        (string path, string componentName, string version) GetVersionedAliasPath(string path)
        {
            var dirPath = Path.GetDirectoryName(path);

            while (dirPath!.Length >= TestSetup.DocsRootPath.Length)
            {
                var dirName = Path.GetFileName(dirPath);
                if (Regex.IsMatch(dirName, @"_(All|\d+(\.\d+)?)$"))
                {
                    var underScoreIndex = dirName.LastIndexOf('_');
                    var componentName = dirName.Substring(0, underScoreIndex);
                    return (dirPath, componentName, dirName.Substring(underScoreIndex + 1));
                }

                dirPath = Path.GetDirectoryName(dirPath);
            }

            return (null, null, null);
        }

        static IEnumerable<XElement> QueryPackageRefs(string projectPath)
        {
            var xdoc = XDocument.Load(projectPath);
            var nsMgr = new XmlNamespaceManager(new NameTable());
            var query = "/Project/ItemGroup/PackageReference";
            var xmlnsAtt = xdoc.Root.Attribute("xmlns");

            if (xmlnsAtt != null)
            {
                nsMgr.AddNamespace("x", xmlnsAtt.Value);
                query = "/x:Project/x:ItemGroup/x:PackageReference";
            }

            var packageRefs = xdoc.XPathSelectElements(query, nsMgr);
            return packageRefs;
        }
    }
}