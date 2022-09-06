using Microsoft.VisualBasic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IntegrityTests
{
    internal class ValidateReferencedPartials
    {
        [Test]
        public void ReferencedPartialsAreUsed()
        {           
            var allErrors = new Dictionary<string, List<string>>();
            var allPartials = Directory.GetFiles(TestSetup.DocsRootPath, "*.partial.md", SearchOption.AllDirectories);
            var allArticles = Directory.GetFiles(TestSetup.DocsRootPath, "*.md", SearchOption.AllDirectories).Except(allPartials);

            foreach (var articlePath in allArticles)
            {
                //Skips the readme file because it references a partial as instructions
                if (articlePath.Contains("README.md"))
                {
                    continue;
                }

                var fileName = Path.GetFileNameWithoutExtension(articlePath);
                string text = File.ReadAllText(articlePath);
                string searchPattern = @"(?i)partial:\s*[a-zA-Z0-9-_]*";
                string replacePattern = @"(?i)partial:\s*";
                var matches = Regex.Matches(text, searchPattern, RegexOptions.IgnoreCase);
                var errors = new List<string>();

                foreach (Match match in matches)
                {
                    var partialName = Regex.Replace(match.Value, replacePattern, "").Trim();

                    if (!allPartials.Any(x => x.Contains(fileName + "_" + partialName)))
                    {
                        errors.Add($"    - '{match.Value}' at postion ({match.Index})");
                    }                
                }

                if (errors.Count > 0)
                {
                    allErrors.TryAdd(articlePath, errors);
                }
            }

            if (allErrors.Count > 0)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($"There is ({allErrors.Count}) markdown file(s) that had references to partials that do not have a corresponding partial file to be used. Here are the file paths and partial references that were not used.\r\n");
                foreach (var error in allErrors)
                {
                    stringBuilder.Append($"\r\n  > {error.Key.ToString()}\r\n");
                    stringBuilder.Append($"{string.Join($"\r\n", error.Value)}");
                }
                Assert.Fail(stringBuilder.ToString());
            }
        }
    }
}
