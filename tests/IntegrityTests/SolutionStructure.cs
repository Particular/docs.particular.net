using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace IntegrityTests
{
    public class SolutionStructure
    {
        [Test]
        public void OneSolutionPerDirectory()
        {
            var directoriesWithMoreThanOneSolution = Directory.GetFiles(TestSetup.DocsRootPath, "*.sln", SearchOption.AllDirectories)
                .Select(Path.GetDirectoryName)
                .Where(dirPath => !dirPath.Contains(Path.Combine("samples", "versioning"))) //Exception for the Versioning sample
                .GroupBy(dirPath => dirPath, StringComparer.OrdinalIgnoreCase)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToArray();

            var errMsg = $"Only one solution file allowed per directory. These have multiple:" + Environment.NewLine
                + string.Join(Environment.NewLine, directoriesWithMoreThanOneSolution);

            Assert.That(directoriesWithMoreThanOneSolution, Is.Empty, errMsg);
        }

        [Test]
        public void OneProjectPerDirectory()
        {
            var directoriesWithMoreThanOneProject = Directory.GetFiles(TestSetup.DocsRootPath, "*.*proj", SearchOption.AllDirectories)
                .Select(Path.GetDirectoryName)
                .GroupBy(dirPath => dirPath, StringComparer.OrdinalIgnoreCase)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToArray();

            var errMsg = $"Only one project file allowed per directory. These have multiple:" + Environment.NewLine
                + string.Join(Environment.NewLine, directoriesWithMoreThanOneProject);

            Assert.That(directoriesWithMoreThanOneProject, Is.Empty, errMsg);
        }
    }
}
