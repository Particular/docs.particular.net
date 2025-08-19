using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Particular.Approvals;

namespace IntegrityTests;

public partial class ClearTextEmails
{
    [Test]
    public void ShouldNotExposeAdditionalCleartextEmails()
    {
        var emails = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        new TestRunner("*.md", "Collect cleartext emails")
            .Run(mdPath =>
            {
                var contents = File.ReadAllText(mdPath);
                var matches = EmailRegex().Matches(contents);
                foreach (Match match in matches)
                {
                    emails.Add(match.Value);
                }

                return true;
            });

        var sorted = emails.OrderBy(str => str).ToList();
        sorted.Insert(0, "This shows the clear-text email addresses exposed on the website. This is bad unless you love spam. The goal is to reduce this list to zero. It should not be allowed to get bigger.");
        Approver.Verify(sorted);
    }

    [GeneratedRegex(@"[\w\.-]+@particular\.net", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex EmailRegex();
}