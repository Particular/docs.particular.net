using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using NUnit.Framework;

namespace IntegrityTests
{
    public class SolutionStructure
    {
        [Test]
        public void OneSolutionPerDirectory()
        {
            var directoriesWithMoreThanOneSolution = Directory.GetFiles(TestSetup.DocsRootPath, "*.sln", SearchOption.AllDirectories)
                .Select(slnPath => Path.GetDirectoryName(slnPath))
                .GroupBy(dirPath => dirPath, StringComparer.OrdinalIgnoreCase)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToArray();

            var errMsg = $"Only one solution file allowed per directory. These have multiple:" + Environment.NewLine
                + string.Join(Environment.NewLine, directoriesWithMoreThanOneSolution);

            Assert.AreEqual(0, directoriesWithMoreThanOneSolution.Length, errMsg);
        }

        [Test]
        public void OneProjectPerDirectory()
        {
            var directoriesWithMoreThanOneProject = Directory.GetFiles(TestSetup.DocsRootPath, "*.*proj", SearchOption.AllDirectories)
                .Select(projPath => Path.GetDirectoryName(projPath))
                .GroupBy(dirPath => dirPath, StringComparer.OrdinalIgnoreCase)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToArray();

            var errMsg = $"Only one project file allowed per directory. These have multiple:" + Environment.NewLine
                + string.Join(Environment.NewLine, directoriesWithMoreThanOneProject);

            Assert.AreEqual(0, directoriesWithMoreThanOneProject.Length, errMsg);
        }
    }
}
