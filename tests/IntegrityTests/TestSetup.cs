using System.IO;
using NUnit.Framework;

namespace IntegrityTests
{
    [SetUpFixture]
    public class TestSetup
    {
        internal static string[] RootDirectories;

        [OneTimeSetUp]
        public void SetupRootDirectories()
        {
            // ENHANCEMENT: Execute git commands when TeamCity environment variables are present
            // to filter the set of files we run against based on changes in a given PR

            var currentDirectory = TestContext.CurrentContext.TestDirectory;
            var docsDirectory = Path.GetFullPath(Path.Combine(currentDirectory, @"..\..\..\..\.."));

            RootDirectories = new[] { docsDirectory };
        }
    }
}