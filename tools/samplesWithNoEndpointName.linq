<Query Kind="Program" />

XNamespace xmlns = "http://schemas.microsoft.com/developer/msbuild/2003";
void Main()
{
	var toolsDiretory = Path.GetDirectoryName(Util.CurrentQueryPath);
	var docsDirectory = Directory.GetParent(toolsDiretory).FullName;
	var samplesdirectory = Path.Combine(docsDirectory, "Samples");
	foreach (var csFile in Directory.EnumerateFiles(samplesdirectory, "*.cs", SearchOption.AllDirectories))
	{

		var classContents = File.ReadAllText(csFile);
		if (!classContents.Contains("new EndpointConfiguration"))
        {
			continue;
		}
		if (classContents.Contains(".EndpointName("))
        {
			continue;
		}
		Debug.WriteLine(csFile);
	}
}