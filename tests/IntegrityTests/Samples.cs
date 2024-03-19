using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace IntegrityTests
{
    internal class Samples
    {
        [Test]
        public void NoMinorSamples()
        {
            const string errorMessage = """
                There should not be samples for different minors within a major version.
                It's not that hard to update to a minor, so users can upgrade to use a new feature if they want it.
                Just be explicit in the sample.md about what minimum version you need to use the code in the sample.
                In the sample projects, reference version Major.* so that the newest minor/patch gets used.
                """;

            new TestRunner("*.sln", errorMessage)
                .IgnoreSnippets()
                .IgnoreTutorials()
                .Run(path =>
                {
                    var versionedDirectory = path.Split(Path.DirectorySeparatorChar).First(part => part.Contains("_"));
                    var parts = versionedDirectory.Split('_');
                    var component = parts[0];
                    var version = parts[1];
                    if (version.Contains('.'))
                    {
                        var major = version.Split('.')[0];
                        return (false, $"Sample directory should be '{component}_{major}', not '{versionedDirectory}");
                    }

                    return (true, null);
                });
        }

        [Test]
        public void TargetFrameworksInDescendingOrder()
        {
            new TestRunner("*.csproj", "Multi-targeted projects should have target frameworks in descending order to make future search/replace operations easier")
                .IgnoreSnippets()
                .IgnoreTutorials()
                .Run(path =>
                {
                    var text = File.ReadAllText(path);
                    var match = Regex.Match(text, "<TargetFrameworks>([^<]+)</TargetFrameworks>");

                    if (match.Success)
                    {
                        var frameworks = match.Groups[1].Value;
                        var frameworkMajors = frameworks.Split(';')
                            .Select(GetMajorFromTargetFramework)
                            .ToArray();

                        for (var i = 0; i < frameworkMajors.Length - 1; i++)
                        {
                            if (frameworkMajors[i] <= frameworkMajors[i + 1])
                            {
                                return (false, $"'{frameworks}' is not in descending order");
                            }
                        }
                    }

                    return (true, null);
                });
        }

        static int GetMajorFromTargetFramework(string tfm)
        {
            if (tfm.StartsWith("net4"))
            {
                return 4;
            }

            var dotIndex = tfm.IndexOf('.');

            if (dotIndex > 0 && tfm.StartsWith("net"))
            {
                var majorText = tfm.Substring(3, dotIndex - 3);
                if (int.TryParse(majorText, out var major))
                {
                    return major;
                }
            }

            throw new Exception($"Couldn't figure out major version from target framework '{tfm}'");
        }
    }
}
