using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace IntegrityTests
{
    internal class ValidateReferencedPartials
    {
        [Test]
        public void ReferencedPartialsAreUsed()
        {
            var allErrors = new Dictionary<string, List<string>>();
            var allPartials = Directory.GetFiles(TestSetup.DocsRootPath, "*.partial.md", SearchOption.AllDirectories);

            // Find the paths of all .md files where the first folder start with .
            var allDotDirectories = Directory.GetDirectories(TestSetup.DocsRootPath, ".*", SearchOption.AllDirectories);
            var allFilesInsideADotDirectory = new List<string>();
            foreach (var directory in allDotDirectories)
            {
                 allFilesInsideADotDirectory.AddRange(Directory.GetFiles(directory, "*.md", SearchOption.AllDirectories));
            }

            var allArticles = Directory.GetFiles(TestSetup.DocsRootPath, "*.md", SearchOption.AllDirectories)
                .Except(allPartials)
                .Except(allFilesInsideADotDirectory);

            foreach (var articlePath in allArticles)
            {
                //Skips the readme file because it references a partial as instructions
                if (articlePath.Contains("README.md"))
                {
                    continue;
                }

                var fileName = Path.GetFileNameWithoutExtension(articlePath);
                string text = File.ReadAllText(articlePath);
                string searchPattern = @"^(?i)partial:\s*[a-zA-Z0-9-]*";
                string replacePattern = @"(?i)partial:\s*";
                var matches = Regex.Matches(text, searchPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                var errors = new List<string>();

                foreach (var match in matches.Cast<Match>())
                {
                    var partialName = Regex.Replace(match.Value, replacePattern, "").Trim();

                    if (!allPartials.Any(x => x.Contains(fileName + "_" + partialName, StringComparison.OrdinalIgnoreCase)))
                    {
                        errors.Add($"    - '{match.Value}' at position ({match.Index})");
                    }
                }

                if (errors.Count > 0)
                {
                    allErrors.TryAdd(articlePath, errors);
                }
            }

            if (allErrors.Count > 0)
            {
                StringBuilder stringBuilder = new();
                stringBuilder.Append($"Found ({allErrors.Count}) markdown file(s) that had references to partials that do not have a corresponding partial file to be used. Here are the file paths and partial references that were not used.\r\n");
                foreach (var error in allErrors)
                {
                    stringBuilder.AppendLine()
                        .AppendLine($"  > {error.Key.ToString()}")
                        .AppendLine($"{string.Join(Environment.NewLine, error.Value)}");
                }
                Assert.Fail(stringBuilder.ToString());
            }
        }
    }
}
