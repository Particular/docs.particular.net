<Query Kind="Program" />

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
				if (solutionFile.IndexOf("VS2015") < 0)
				{
					if (line.StartsWith("# Visual Studio "))
					{
						//VS 2019
						writer.WriteLine("# Visual Studio Version 16");
						continue;
					}
					//https://www.visualstudio.com/en-us/news/releasenotes/vs2017-relnotes
					if (line.StartsWith("MinimumVisualStudioVersion = "))
					{
						writer.WriteLine("MinimumVisualStudioVersion = 15.0.26730.12");
						continue;
					}
				}
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
    var noBomUtf8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
    
	foreach (var projectFile in Directory.EnumerateFiles(docsDirectory, "*.csproj", SearchOption.AllDirectories))
	{
		var xdocument = XDocument.Load(projectFile, LoadOptions.PreserveWhitespace);

		var propertyGroup = xdocument.Descendants("PropertyGroup").FirstOrDefault();

		if (propertyGroup != null)
		{
			var langVersion = propertyGroup.Element("LangVersion");
            
			if (langVersion == null)
			{
				propertyGroup.Add(new XElement("LangVersion", "7.3"));
			}
			else
			{
				langVersion.Value = "7.3";
			}

            var targetFrameworks = propertyGroup.Element("TargetFrameworks");

            if (targetFrameworks != null && !targetFrameworks.Value.Contains(";"))
            {
                targetFrameworks.AddAfterSelf(new XElement("TargetFramework", targetFrameworks.Value));
                targetFrameworks.Remove();
            }
        }
        
        var settings = new XmlWriterSettings { Encoding = noBomUtf8Encoding, OmitXmlDeclaration = true };
        using (var writer = XmlWriter.Create(projectFile, settings))
        {
            xdocument.Save(writer);
        }
     
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
		var solutionName = Path.GetFileNameWithoutExtension(solutionFile);
		foreach (string file in Directory.GetFiles(solutionDirectory, $"*{solutionName}.sln.DotSettings"))
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
	var doc = new XmlDocument();
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