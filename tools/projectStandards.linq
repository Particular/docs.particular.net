<Query Kind="Program">
  <NuGetReference>SetStartupProjects</NuGetReference>
  <Namespace>SetStartupProjects</Namespace>
</Query>

void Main()
{
	var toolsDiretory = Path.GetDirectoryName(Util.CurrentQueryPath);
	var docsDirectory = Directory.GetParent(toolsDiretory).FullName;
	var startProjectSuoCreator = new StartProjectSuoCreator();
	foreach (var projectFile in Directory.EnumerateFiles(docsDirectory, "*.csproj", SearchOption.AllDirectories))
	{
		var projectText = File.ReadAllText(projectFile);
		if (!projectText.Contains("<UseVSHostingProcess>false</UseVSHostingProcess>"))
		{
			throw new Exception("Remove VS host : " + projectFile);
		}
	}
}