using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace IntegrityTests
{
    public class UpgradeGuides
    {
        static IDeserializer deserializer;
        static Regex yamlSeparatorRegex = new Regex("---");
        static string upgradesPathPart = $"{Path.DirectorySeparatorChar}upgrades{Path.DirectorySeparatorChar}";

        static UpgradeGuides()
        {
            var builder = new DeserializerBuilder();
            builder.IgnoreUnmatchedProperties();
            builder.WithNamingConvention(CamelCaseNamingConvention.Instance);
            deserializer = builder.Build();
        }

        [Test]
        public void VerifyUpgradeGuidesDontDoubleMentionCoreVersions()
        {
            new TestRunner("*.md", "Mentioning the same core version twice in upgrade guide metadata is invalid")
                .IgnoreSnippets()
                .IgnoreTutorials()
                .IgnoreRegex(@".*\.include\.md")
                .Run(path =>
                {
                    // This test is important because if the same Core version is entered twice (i.e. versions 7 and 7) then that
                    // upgrade guide would erroneously be added to the Core7to8 upgrade guide.
                    // NOTE: It may be possible to add this test to the engine in the future and then remove this integrity test.
                    try
                    {
                        if (!path.Contains(upgradesPathPart))
                        {
                            return true;
                        }

                        var markdown = File.ReadAllText(path);
                        var separatorMatches = yamlSeparatorRegex.Matches(markdown);
                        var yaml = markdown.Substring(separatorMatches[0].Index + 3, separatorMatches[1].Index - 3).Trim();
                        var metadata = deserializer.Deserialize<Metadata>(yaml);

                        var coreVersions = metadata?.UpgradeGuideCoreVersions;

                        if (coreVersions == null)
                        {
                            return true;
                        }

                        if (coreVersions.Length == 1)
                        {
                            return true;
                        }

                        if (coreVersions.Length != coreVersions.Distinct().Count())
                        {
                            return false;
                        }

                        return true;
                    }
                    catch (System.Exception x)
                    {
                        System.Console.WriteLine(x);
                        return false;
                    }
                });
        }

        class Metadata
        {
            public int[] UpgradeGuideCoreVersions { get; set; }
        }
    }
}
