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
            var currentDirectory = TestContext.CurrentContext.TestDirectory;
            var docsDirectory = Path.GetFullPath(Path.Combine(currentDirectory, @"..\..\..\..\.."));

            RootDirectories = new[] { docsDirectory };
        }
    }
}