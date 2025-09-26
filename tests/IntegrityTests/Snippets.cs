using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace IntegrityTests;

public class Snippets
{
    [Test]
    public void NoSnippetsForMinorVersions()
    {
        const string errorMessage = """
                                    There should not be snippets for different minors within a major version.
                                    Once the new minor exists, it is the "best supported" version for that major and users should use it.
                                    It's also *very* difficult to unwind minor-specific snippets when we create the next major version, and
                                    they tend to break upgrade guide pages without you even realizing it's happening.
                                    Instead, for new features, add text in italics like _Added in version 5.5.0_`
                                    """;

        new TestRunner("*.sln", errorMessage)
            .IgnoreSamples()
            .IgnoreTutorials()
            .Run(path =>
            {
                var directoryPath = Path.GetDirectoryName(path);
                var incorrect = Directory.GetDirectories(directoryPath)
                    .Where(p => p.Contains("_"))
                    .Select(versionedPath => Path.GetFileName(versionedPath))
                    .Where(name => Regex.IsMatch(name, @"\d+\.\d+$"))
                    .ToArray();

                if (incorrect.Any())
                {
                    string incorrectVersions = string.Join(", ", incorrect);
                    return (false, $"Invalid snippet directories based on minor versions {incorrectVersions} which must be moved to the snippet directory for the corresponding major version(s).");
                }

                return (true, null);
            });
    }

    [Test]
    public void NoMinorVersionsInPartialBoundaries()
    {
        const string errorMessage = "Do not use minor versions in partial version boundaries, only majors as integers, i.e. [6,7) [5,) [,9) etc.";

        new TestRunner("*.partial.md", errorMessage)
            .Run(path =>
            {
                var filename = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(path));
                var partialNameSplit = filename.Split('_');
                var versionPart = partialNameSplit[3];
                Console.WriteLine(versionPart);

                return !versionPart.Contains(".");
            });
    }
}