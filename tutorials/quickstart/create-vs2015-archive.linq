<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.IO.Compression.FileSystem.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.IO.Compression.dll</Reference>
  <NuGetReference>SetStartupProjects</NuGetReference>
  <Namespace>SetStartupProjects</Namespace>
  <Namespace>System.IO.Compression</Namespace>
</Query>

void Main()
{
    var suoCreator = new StartProjectSuoCreator();
    var startProjectsFinder = new StartProjectFinder();
    
    var workingDir = Path.GetDirectoryName(Util.CurrentQueryPath);
    var docsRoot = Path.GetFullPath(Path.Combine(workingDir, "..\\.."));
    var solutionDir = Path.Combine(workingDir, "VS2015Solution");
    var solutionPath = Path.Combine(solutionDir, "RetailDemo.sln");
    var zipPath = Path.Combine(workingDir, "tutorials-quickstart-vs2015.zip");
    var tmpDirectory = GetTemporaryDirectory();
    var tmpSolutionPath = Path.Combine(tmpDirectory, "RetailDemo.sln");
    
    var gitClean = new ProcessStartInfo("git", "clean -xfd tutorials/quickstart/VS2015Solution");
    gitClean.WorkingDirectory = docsRoot;
    Process.Start(gitClean).WaitForExit();

    try
    {
        CopyFilesRecursively(solutionDir, tmpDirectory, DirectoryExclusion);
        ReplaceDotSettings(tmpDirectory, docsRoot);
        
        // Include NuGet Config
        File.Copy(Path.Combine(docsRoot, "nuget.config"), Path.Combine(tmpDirectory, "nuget.config"), true);
        
        // Startup Projects
        var startProjects = startProjectsFinder.GetStartProjects(solutionPath).ToList();
        suoCreator.CreateForSolutionFile(tmpSolutionPath, startProjects, VisualStudioVersions.Vs2015);
        
        ZipFile.CreateFromDirectory(tmpDirectory, zipPath, CompressionLevel.Fastest, false);
    }
    finally
    {
        Directory.Delete(tmpDirectory, true);   
    }
}

public static void CopyFilesRecursively(string source, string target, Func<string, bool> directoryExclusion)
{
    CopyFilesRecursively(new DirectoryInfo(source), new DirectoryInfo(target), directoryExclusion);
}

static bool DirectoryExclusion(string directoryName)
{
    var suffix = Path.GetFileName(directoryName);
    return
        suffix == "ArtifactDependencies" ||
        suffix == "BuildOutput" ||
        suffix == "bin" ||
        suffix == "packages" ||
        suffix == "obj" ||
        suffix == ".learningtransport";
}

static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target, Func<string, bool> directoryExclusion)
{
    foreach (var dir in source.GetDirectories())
    {
        if (directoryExclusion(dir.FullName))
        {
            continue;
        }
        DirectoryInfo directoryInfo;
        try
        {
            directoryInfo = target.CreateSubdirectory(dir.Name);
        }
        catch (Exception exception)
        {
            throw new Exception(dir.Name, exception);
        }
        CopyFilesRecursively(dir, directoryInfo, directoryExclusion);
    }
    foreach (var file in source.GetFiles())
    {
        if (file.Name.EndsWith(".md"))
        {
            continue;
        }
        file.CopyTo(Path.Combine(target.FullName, file.Name));
    }
}

public static string GetTemporaryDirectory()
{
    var tempDirectory = Path.Combine(Path.GetTempPath(), "DocsTemp", Path.GetRandomFileName());
    Directory.CreateDirectory(tempDirectory);
    return tempDirectory;
}

static void ReplaceDotSettings(string tempDirectory, string docsRoot)
{
    var settingsTemplate = Path.Combine(docsRoot, @"tools\Shared.DotSettings");
    foreach (var solutionFile in Directory.EnumerateFiles(tempDirectory, "*.sln", SearchOption.AllDirectories))
    {
        File.Copy(settingsTemplate, $"{solutionFile}.DotSettings", true);
    }
}