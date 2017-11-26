<Query Kind="Program">
  <NuGetReference>SetStartupProjects</NuGetReference>
  <Namespace>SetStartupProjects</Namespace>
</Query>

void Main()
{
	var toolsDiretory = Path.GetDirectoryName(Util.CurrentQueryPath);
	var docsDirectory = Directory.GetParent(toolsDiretory).FullName;
	var samples = Path.Combine(docsDirectory, "Samples");
	SetStartupProjects(samples);
}

public static void SetStartupProjects(string codeDirectory)
{
	foreach (var suo in Directory.EnumerateDirectories(codeDirectory, ".vs", SearchOption.AllDirectories))
	{
		Directory.Delete(suo, true);
	}
	foreach (var suo in Directory.EnumerateFiles(codeDirectory, "*.suo", SearchOption.AllDirectories))
	{
		File.Delete(suo);
	}
	var startProjectSuoCreator = new StartProjectSuoCreator();
	foreach (var solutionFile in Directory.EnumerateFiles(codeDirectory, "*.sln", SearchOption.AllDirectories))
	{
		var startProjectFinder = new StartProjectFinder();
		var startProjects = startProjectFinder.GetStartProjects(solutionFile).ToList();
		if (startProjects.Any())
		{
			startProjectSuoCreator.CreateForSolutionFile(solutionFile, startProjects, VisualStudioVersions.Vs2017);
		}
	}
}
