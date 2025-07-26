using System;
using System.IO;
using System.Linq;

namespace Shared
{
    public class SolutionDirectoryFinder
    {
        public string Root { get; }

        public SolutionDirectoryFinder(string root) => Root = root;

        public SolutionDirectoryFinder()
        {
            var directory = AppContext.BaseDirectory;

            while (true)
            {
                if (Directory.EnumerateFiles(directory).Any(file => file.EndsWith(".sln")))
                {
                    Root = directory;
                    return;
                }

                var parent = Directory.GetParent(directory) ?? throw new Exception(
                    "Couldn't find the solution directory for the ClaimCheck storage. " +
                    "If the endpoint is outside the solution folder structure, " +
                    "make sure to specify a storage directory using an absolute path.");

                directory = parent.FullName;
            }
        }

        public string GetDirectory(string relativePath)
        {
            var fullPath = Path.GetFullPath(Path.Combine(Root, relativePath));
            Directory.CreateDirectory(fullPath);
            return fullPath;
        }
    }
}
