<Query Kind="Program" />

XNamespace xmlns = "http://schemas.microsoft.com/developer/msbuild/2003";

string toolsDiretory = Path.GetDirectoryName(Util.CurrentQueryPath);
string docsDirectory = Directory.GetParent(Path.GetDirectoryName(Util.CurrentQueryPath)).FullName;

void Main()
{
	FixResharperSettings();
	CleanUpSolutions();
	CleanUpProjects();
	DeleteAssemblyInfo();
}

void DeleteAssemblyInfo()
{
	foreach (var assemblyInfo in Directory.EnumerateFiles(docsDirectory, "AssemblyInfo.cs", SearchOption.AllDirectories))
	{
		if (Path.GetDirectoryName(assemblyInfo).EndsWith("Properties"))
		{
			File.Delete(assemblyInfo);
		}
	}
}

void CleanUpSolutions()
{
	foreach (var solutionFile in Directory.EnumerateFiles(docsDirectory, "*.sln", SearchOption.AllDirectories))
	{
		var lines = File.ReadAllLines(solutionFile);
		File.Delete(solutionFile);
		using (var writer = new StreamWriter(solutionFile))
		{
			foreach (var line in lines)
			{
				if (line.Contains(".Release"))
				{
					continue;
				}
				if (line.Contains("Release|"))
				{
					continue;
				}
				writer.WriteLine(line);
			}
		}
	}
}

void CleanUpProjects()
{
	foreach (var projectFile in Directory.EnumerateFiles(docsDirectory, "*.csproj", SearchOption.AllDirectories))
	{
		var xdocument = XDocument.Load(projectFile);

		var propertyGroup = xdocument.Descendants("PropertyGroup").FirstOrDefault();

		if (propertyGroup != null)
		{
			var langVersion = propertyGroup.Element("LangVersion");
			if (langVersion == null)
			{
				propertyGroup.Add(new XElement("LangVersion", "6"));
			}
			else
			{
				langVersion.Value = "6";
			}
		}
		xdocument.Save(projectFile);
		CollapseEmptyElements(projectFile);
	}
}

void FixResharperSettings()
{
	var sharedSettings = Path.Combine(toolsDiretory, "Shared.DotSettings");
	var layeredSettings = Path.Combine(toolsDiretory, "Layered.DotSettings");
	var layeredText = File.ReadAllText(layeredSettings);
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


static void CollapseEmptyElements(string file)
{
	XmlDocument doc = new XmlDocument();
	doc.Load(file);
	CollapseEmptyElements(doc.DocumentElement);
	doc.Save(file);
}

static void CollapseEmptyElements(XmlElement node)
{
	if (!node.IsEmpty && node.ChildNodes.Count == 0)
	{
		node.IsEmpty = true;
	}

	foreach (XmlNode child in node.ChildNodes)
	{
		if (child.NodeType == XmlNodeType.Element)
		{
			CollapseEmptyElements((XmlElement)child);
		}
	}
}