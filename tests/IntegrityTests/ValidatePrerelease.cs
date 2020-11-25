using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using NUnit.Framework;

namespace IntegrityTests
{
    public class ValidatePrerelease
    {
        [Test]
        public void VerifyPrereleaseTxtHasPrereleaseComponent()
        {
            new TestRunner("prerelease.txt", "Found prerelease.txt used on a versioned component directory containing no projects using a prerelease version of that component.")
                .Run(path =>
                {
                    var versionedPath = Path.GetDirectoryName(path);
                    var versionedName = Path.GetFileName(versionedPath);
                    var versionSplit = versionedName.Split('_');
                    var aliasName = versionSplit[0];

                    if (!TestSetup.NugetAliases.TryGetValue(aliasName, out var nugetName))
                    {
                        Assert.Warn($"Nuget for alias name '{aliasName}' was not found.");
                    }

                    var componentMetadata = TestSetup.ComponentMetadata.Select(c => c.Value)
                        .FirstOrDefault(c => c.NugetOrder.Contains(nugetName, StringComparer.OrdinalIgnoreCase));

                    if (componentMetadata == null)
                    {
                        Assert.Warn($"Component metadata for alias name '{aliasName}' and nuget name '{nugetName}' was not found.");
                    }

                    var packageNames = componentMetadata.NugetOrder;

                    var directoryPath = Path.GetDirectoryName(path);
                    var projects = Directory.GetFiles(directoryPath, "*.csproj", SearchOption.AllDirectories);

                    return projects.Any(projectPath => HasPrereleasePackages(projectPath, packageNames));
                });
        }

        [Test]
        public void VerifyProjectUsingPrereleaseComponentMarkedWithPrereleaseTxt()
        {
            new TestRunner("*.csproj", "Found project with prerelease component not marked by a prerelease.txt file.")
                .IgnoreTutorials()
                .IgnoreRegex(@"\\Snippets\\Common\\")
                .Run(projPath =>
                {
                    var (versionedPath, aliasName) = GetVersionedAliasPath(projPath);

                    if (versionedPath == null)
                    {
                        return false;
                    }

                    // Temporarily skipping over, this probably means incorrect component names, but that's another test
                    if (!TestSetup.NugetAliases.TryGetValue(aliasName, out var nugetName))
                    {
                        return true;
                    }

                    var component = TestSetup.ComponentMetadata.Select(c => c.Value)
                        .FirstOrDefault(c => c.NugetOrder.Contains(nugetName, StringComparer.OrdinalIgnoreCase));

                    // Temporarily skipping over, this probably means incorrect component names, but that's another test
                    if (component == null)
                    {
                        return true;
                    }

                    if (HasPrereleasePackages(projPath, component.NugetOrder))
                    {
                        var prereleasePath = Path.Combine(versionedPath, "prerelease.txt");
                        return File.Exists(prereleasePath);
                    }

                    return true;
                });
        }

        [Test]
        public void VerifyProjectsWithPrereleaseDontUseWildcards()
        {
            new TestRunner("*.csproj", "Found project with prerelease component using a wildcard nuget pattern.")
                .IgnoreTutorials()
                .IgnoreRegex(@"\\Snippets\\Common\\")
                .Run(projPath =>
                {
                    var (versionedPath, aliasName) = GetVersionedAliasPath(projPath);

                    if (versionedPath == null)
                    {
                        return false;
                    }

                    // Temporarily skipping over, this probably means incorrect component names, but that's another test
                    if (!TestSetup.NugetAliases.TryGetValue(aliasName, out var nugetName))
                    {
                        return true;
                    }

                    var component = TestSetup.ComponentMetadata.Select(c => c.Value)
                        .FirstOrDefault(c => c.NugetOrder.Contains(nugetName, StringComparer.OrdinalIgnoreCase));

                    // Temporarily skipping over, this probably means incorrect component names, but that's another test
                    if (component == null)
                    {
                        return true;
                    }

                    if (HasPrereleasePackagesNotLockedToSpecificVersion(projPath, component.NugetOrder))
                    {
                        return false;
                    }

                    return true;
                });
        }

        private (string path, string componentName) GetVersionedAliasPath(string path)
        {
            var dirPath = Path.GetDirectoryName(path);

            while (dirPath.Length >= TestSetup.DocsRootPath.Length)
            {
                var dirName = Path.GetFileName(dirPath);
                if (Regex.IsMatch(dirName, @"_(All|\d+(\.\d+)?)$"))
                {
                    var componentName = dirName.Substring(0, dirName.IndexOf('_'));
                    return (dirPath, componentName);
                }

                dirPath = Path.GetDirectoryName(dirPath);
            }

            return (null, null);
        }

        private static bool HasPrereleasePackages(string projectPath, IEnumerable<string> packageNames)
        {
            var packageRefs = QueryPackageRefs(projectPath);

            return packageRefs
                .Where(pkgRef => packageNames.Contains(pkgRef.Attribute("Include").Value, StringComparer.OrdinalIgnoreCase))
                .Any(pkgRef => pkgRef.Attribute("Version").Value.Contains("-"));
        }

        private static bool HasPrereleasePackagesNotLockedToSpecificVersion(string projectPath, IEnumerable<string> packageNames)
        {
            var packageRefs = QueryPackageRefs(projectPath);

            return packageRefs
                .Where(pkgRef => packageNames.Contains(pkgRef.Attribute("Include").Value, StringComparer.OrdinalIgnoreCase) && pkgRef.Attribute("Version").Value.Contains("-"))
                .Any(pkgRef =>  pkgRef.Attribute("Version").Value.Contains("*"));
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
