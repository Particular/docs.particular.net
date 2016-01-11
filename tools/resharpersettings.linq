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
	var layeredText=File.ReadAllText(layeredSettings);
	var startProjectSuoCreator = new StartProjectSuoCreator();
	foreach (var solutionFile in Directory.EnumerateFiles(docsDirectory, "*.sln", SearchOption.AllDirectories))
	{
		var solutionDirectory = Path.GetDirectoryName(solutionFile);
		foreach (string file in Directory.GetFiles(solutionDirectory, "*.DotSettings"))
        {
			File.Delete(file);
		}
		var relative = GetRelativePath(sharedSettings, solutionDirectory);
		var replaced = layeredText.Replace("SharedDotSettings", relative);
		var targetFile = solutionFile + ".DotSettings";
		File.WriteAllText(targetFile, replaced);
	}
}

string GetRelativePath(string filespec, string folder)
{
	Uri pathUri = new Uri(filespec);
	// Folders must end in a slash
	if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
	{
		folder += Path.DirectorySeparatorChar;
	}
	Uri folderUri = new Uri(folder);
	return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
}