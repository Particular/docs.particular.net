using System;
using System.IO;

namespace Shared;

public static class SolutionDirectoryFinder
{
    public static string Find(string solutionRelativePath = null)
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (directory != null && !ContainsSolutionFile(directory))
        {
            directory = directory.Parent;
        }

        if (directory == null)
        {
            throw new Exception("Could not find solution directory");
        }

        if (string.IsNullOrEmpty(solutionRelativePath))
        {
            return directory.FullName;
        }

        return Path.Combine(directory.FullName, solutionRelativePath);
    }

    static bool ContainsSolutionFile(DirectoryInfo directory)
    {
        return directory.GetFiles("*.sln").Length > 0;
    }
}
