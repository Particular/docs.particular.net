<Query Kind="Program">
  <NuGetReference>SetStartupProjects</NuGetReference>
  <Namespace>SetStartupProjects</Namespace>
</Query>

void Main()
{
	var toolsDiretory = Path.GetDirectoryName(Util.CurrentQueryPath);
	var docsDirectory = Directory.GetParent(toolsDiretory).FullName;
	var sharedSettings = Path.Combine(toolsDiretory, "Shared.DotSettings");
	var layeredSettings =Path.Combine(toolsDiretory, "Layered.DotSettings");

	var startProjectSuoCreator = new StartProjectSuoCreator();
	foreach (var solutionFile in Directory.EnumerateFiles(docsDirectory, "*.sln", SearchOption.AllDirectories))
	{
		var targetFile =Path.Combine(solutionFile, ".DotSettings")
    }
}

