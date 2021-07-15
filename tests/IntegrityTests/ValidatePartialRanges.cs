using NuGet.Versioning;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace IntegrityTests
{
    public class ValidatePartialRanges
    {
        [Test]
        public void PartialRangesDoNotOverlap()
        {
            var errors = new List<string>();

            var partialGroups = Directory.GetFiles(TestSetup.DocsRootPath, "*.partial.md", SearchOption.AllDirectories)
                .Select(path =>
                {
                    var filename = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(path));
                    var parts = filename.Split('_');

                    if (parts.Length != 4)
                    {
                        errors.Add($"Partial {path} is invalid. Its name does not have 4 segments.");
                        return null;
                    }

                    if (!TryParseVersionRange(parts[3], out var versionRange))
                    {
                        errors.Add($"Partial {path} is invalid. Cannot parse the version into a valid VersionRange.");
                        return null;
                    }

                    return new
                    {
                        Path = path,
                        DirectoryPath = Path.GetDirectoryName(path),
                        ParentFile = parts[0],
                        Name = parts[1],
                        Component = parts[2],
                        VersionsText = parts[3],
                        Versions = versionRange
                    };
                })
                .Where(partial => partial != null)
                .GroupBy(partial => new { partial.DirectoryPath, partial.ParentFile, partial.Name, partial.Component })
                // If only one partial, there can't be an overlap
                .Where(group => group.Take(1).Any());

            foreach (var group in partialGroups)
            {
                var partials = group.ToArray();

                for (int i = 0; i < partials.Length; i++)
                {
                    for (int j = i + 1; j < partials.Length; j++)
                    {
                        var left = partials[i];
                        var right = partials[j];
                        var common = VersionRange.CommonSubSet(new[] { left.Versions, right.Versions });
                        if (common != VersionRange.None)
                        {
                            var k = group.Key;
                            errors.Add($"In {k.DirectoryPath}, there is a version overlap in partials for {k.ParentFile}_{k.Name}_{k.Component}:\r\n    - Partial Versions: '{left.VersionsText}' and '{right.VersionsText}'\r\n    - Parsed Versions: '{left.Versions}' and '{right.Versions}'");
                        }
                    }
                }
            }

            if (errors.Count > 0)
            {
                Assert.Fail($"Problems detected with partial version ranges which can cause the wrong partial to be rendered. Remember that upper ranges should use ) for an exclusive range, not ] which indicates an inclusive range.\r\n  > {string.Join("\r\n  > ", errors)}");
            }
        }

        static bool TryParseVersionRange(string stringVersion, out VersionRange parsedVersion)
        {
            if (int.TryParse(stringVersion, out var majorPart))
            {
                parsedVersion = new VersionRange(
                    minVersion: new NuGetVersion(majorPart, 0, 0),
                    includeMinVersion: true,
                    maxVersion: new NuGetVersion(majorPart + 1, 0, 0),
                    includeMaxVersion: false);
                return true;
            }
            if (NuGetVersion.TryParse(stringVersion, out var minVersion))
            {
                parsedVersion = new VersionRange(
                    minVersion: minVersion,
                    includeMinVersion: true,
                    maxVersion: new NuGetVersion(minVersion.Major + 1, 0, 0),
                    includeMaxVersion: false);
                return true;
            }
            return VersionRange.TryParse(stringVersion, out parsedVersion);
        }
    }
}
