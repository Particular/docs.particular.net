using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using NUnit.Framework;

namespace IntegrityTests
{
    public class ProjectLangVersion
    {
        [Test]
        public void SnippetsAndTutorialsShouldNotHaveLangVersionProperty()
        {
            new TestRunner("*.csproj", "Snippets and Tutorial projects should not have a LangVersion property")
                .IgnoreSamples()
                .Run(projectFilePath =>
                {
                    var xdoc = XDocument.Load(projectFilePath);
                    if (xdoc.Root.Attribute("xmlns") != null)
                    {
                        return true;
                    }

                    var firstLangVersionElement = xdoc.XPathSelectElement("/Project/PropertyGroup/LangVersion");

                    return firstLangVersionElement is null;
                });
        }

        [Test]
        public void SamplesShouldUseCorrectLangVersion()
        {
            new TestRunner("*.sln", "Samples must specify the correct LangVersion")
                .IgnoreSnippets()
                .IgnoreTutorials()
                .Run(solutionFilePath =>
                {
                    var samplePath = Path.GetDirectoryName(solutionFilePath);
                    var projectFiles = Directory.GetFiles(samplePath, "*.csproj", SearchOption.AllDirectories);

                    int? solutionLangVersion = null;

                    foreach (var projectFile in projectFiles)
                    {
                        //Don't analyze generated project files
                        if (projectFile.Contains("obj"))
                        {
                            continue;
                        }

                        var doc = XDocument.Load(projectFile);

                        if (doc.Root.Attribute("Sdk") == null)
                        {
                            continue;
                        }

                        string[] projectTfms;

                        var firstTargetFrameworkElement = doc.XPathSelectElement("/Project/PropertyGroup/TargetFramework");
                        var tfm = firstTargetFrameworkElement?.Value;

                        if (tfm is not null)
                        {
                            projectTfms = [tfm];
                        }
                        else
                        {
                            var firstTargetFrameworksElement = doc.XPathSelectElement("/Project/PropertyGroup/TargetFrameworks");
                            var tfms = firstTargetFrameworksElement?.Value;

                            projectTfms = tfms.Split(';');
                        }

                        var projectLangVersion = GetLangVersionForProject(projectTfms);

                        if (solutionLangVersion is null || projectLangVersion < solutionLangVersion)
                        {
                            solutionLangVersion = projectLangVersion;
                        }
                    }

                    solutionLangVersion ??= fallbackLangVersion;

                    var solutionLangVersionString = solutionLangVersion > latestReleasedLangVersion ? "preview" : solutionLangVersion.Value.ToString("N1");

                    bool valid = true;

                    foreach (var projectFile in projectFiles)
                    {
                        //Don't analyze generated project files
                        if (projectFile.Contains("obj"))
                        {
                            continue;
                        }

                        var doc = XDocument.Load(projectFile);

                        if (doc.Root.Attribute("Sdk") == null)
                        {
                            continue;
                        }

                        var firstLangVersionElement = doc.XPathSelectElement("/Project/PropertyGroup/LangVersion");
                        var langVersion = firstLangVersionElement?.Value ?? "Unknown";

                        if (!langVersion.Equals(solutionLangVersionString, System.StringComparison.Ordinal))
                        {
                            valid = false;
                        }
                    }

                    return (valid, $"LangVersion should be {solutionLangVersionString}");
                });
        }

        static int? GetLangVersionForProject(string[] tfms)
        {
            int? projectLangVersion = null;

            foreach (var tfm in tfms)
            {
                if (langVersions.TryGetValue(tfm, out var langVersion))
                {
                    if (projectLangVersion is null || langVersion < projectLangVersion)
                    {
                        projectLangVersion = langVersion;
                    }
                }
            }

            return projectLangVersion;
        }

        [Test]
        public void ShouldHaveLangVersionMappingForEachAllowedTargetFramework()
        {
            List<string> missingTargetFrameworks = [];

            foreach (var tfm in ProjectFrameworks.sdkProjectAllowedTfmList)
            {
                if (!langVersions.ContainsKey(tfm))
                {
                    missingTargetFrameworks.Add(tfm);
                }
            }

            if (missingTargetFrameworks.Count > 0)
            {
                Assert.Fail($"The following allowed TargetFrameworks are missing LangVersion mappings:\r\n  > {string.Join("\r\n  > ", missingTargetFrameworks)}");
            }
        }

        static readonly int fallbackLangVersion = 10;

        static readonly Dictionary<string, int?> langVersions = new()
        {
            // null values here mean we don't want that tfm to be considered in the calculations
            {"net48", null },
            {"netstandard2.0", null },
            { "net8.0", 12 },
            { "net8.0-windows", 12 },
            { "net9.0", 13 },
            { "net9.0-windows", 13 },
            { "net10.0", 14 },
            { "net10.0-windows", 14 }
        };

        // This needs to be kept up to date with the latest released version of C#
        private const int latestReleasedLangVersion = 14;
    }
}
