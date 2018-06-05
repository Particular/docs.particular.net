using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace IntegrityTests
{
    public class ValidateWarningInsteadOfWarn
    {
        [Test]
        public void VerifyWarningIsUsedInsteadOfWarn()
        {
            new TestRunner("*.md", "Found WARN instead of WARNING.")
                .Run(path => !File.ReadLines(path).Any(l => l.Contains("WARN:")));
        }
    }
}