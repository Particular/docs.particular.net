using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace IntegrityTests;

partial class MergeConflicts
{

    [Test]
    public void EnsureNoMergeConflictRemnants()
    {
        new TestRunner("*.md", "Looks like some remnant of a merge conflict remains in a markdown file")
            .IgnoreSnippets()
            .Run(path =>
            {
                return !RegexInstance.IsMatch(File.ReadAllText(path));
            });
    }

    [GeneratedRegex(@"[<=>]{4,}")]
    private static partial Regex MergeConflictRegex();

    private static readonly Regex RegexInstance = MergeConflictRegex();
}
