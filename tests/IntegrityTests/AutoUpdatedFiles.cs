using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace IntegrityTests
{
    class AutoUpdatedFiles
    {
        [Test]
        public void EnsureUpdatedFilesExist()
        {
            string[] paths = [
                "nservicebus/upgrades/supported-versions-nservicebus.include.md",
                "nservicebus/upgrades/supported-versions-downstreams.include.md",
                "nservicebus/upgrades/all-versions.include.md",
                "servicecontrol/upgrades/supported-versions-servicecontrol.include.md",
            ];

            Assert.Multiple(() =>
            {
                foreach (var path in paths)
                {
                    var fullPath = Path.GetFullPath(Path.Combine(TestSetup.DocsRootPath, path));
                    var fileInfo = new FileInfo(fullPath);

                    Assert.That(fileInfo.Exists, Is.True, $"Docs path '{path}' must exist because it is automatically updated by a scheduled process, and can't be moved.");
                    if (fileInfo.Exists)
                    {
                        Assert.That(fileInfo.Length, Is.GreaterThan(200), $"Docs path '{path}' exists but seems suspiciously small. This might be indicative of a problem.");
                    }
                }
            });
        }
    }
}
