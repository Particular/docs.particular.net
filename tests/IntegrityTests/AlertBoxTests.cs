using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace IntegrityTests;

internal partial class AlertBoxTests
{
    [Test]
    public void NoLegacyAlertBoxes()
    {
        new TestRunner("*.md", "Old-style alert boxes no longer supported, use GitHub style alert boxes")
            .Run(path =>
            {
                var matches = LegacyAlertRegex().Matches(File.ReadAllText(path))
                    .OfType<Match>()
                    .Select(match => match.Groups[2].Value)
                    .Where(val => val is not "info" and not "warn") // This can be in .NET logging output with exactly this casing
                    .ToArray();

                if (matches.Any())
                {
                    var msg = "Found legacy alert(s) of type " + string.Join(" & ", matches);
                    return (false, msg);
                }

                return (true, null);
            });
    }

    [GeneratedRegex(@"^(\{\{)?(SUCCESS|NOTE|INFO|WARN|WARNING|DANGER):", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase)]
    private partial Regex LegacyAlertRegex();

    [Test]
    public void AlertBoxesAreValid()
    {
        var only = string.Join(" ", ValidGitHubAlertTypes.Select(s => $"![{s}]"));

        new TestRunner("*.md", $"Invalid alert boxes found, only {only} are valid")
            .Run(path =>
            {
                var invalidTypes = GitHubAlertBoxHeaderRegex().Matches(File.ReadAllText(path))
                    .OfType<Match>()
                    .Select(m => m.Groups[1].Value)
                    .Where(type => !ValidGitHubAlertTypes.Contains(type))
                    .Distinct()
                    .ToArray();

                if (invalidTypes.Any())
                {
                    var typesStr = string.Join(" & ", invalidTypes.Select(s => $"![{s}]"));
                    var msg = $"Alert box of type {typesStr} is invalid";
                    return (false, msg);
                }

                return (true, null);
            });
    }

    [GeneratedRegex(@"^> \[!(\w+)\]\s*\r?\n", RegexOptions.Compiled | RegexOptions.Multiline)]
    private partial Regex GitHubAlertBoxHeaderRegex();

    static readonly HashSet<string> ValidGitHubAlertTypes = ["NOTE", "TIP", "IMPORTANT", "WARNING", "CAUTION"];
}
