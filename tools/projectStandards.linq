<Query Kind="Program" />

XNamespace xmlns = "http://schemas.microsoft.com/developer/msbuild/2003";
void Main()
{
	var toolsDiretory = Path.GetDirectoryName(Util.CurrentQueryPath);
	var docsDirectory = Directory.GetParent(toolsDiretory).FullName;

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
					//continue;
				}
				writer.WriteLine(line);
			}
		}
	}
	return;
	foreach (var projectFile in Directory.EnumerateFiles(docsDirectory, "*.csproj", SearchOption.AllDirectories))
	{
		var xdocument = XDocument.Load(projectFile);
		var propertyGroups = xdocument.Descendants(xmlns + "PropertyGroup").ToList();

		xdocument.Descendants(xmlns + "NuGetPackageImportStamp")
			.Remove();

		foreach (var element in propertyGroups)
		{ 
			var condition = element.Attribute("Condition");
			if (condition == null) 
			{
				continue;
			}

			if (condition.Value == " '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ")
			{
				element.Remove();
			}

			if (condition.Value != " '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ")
			{
				var treatAsErrors = element.Element(xmlns + "TreatWarningsAsErrors");
				if (treatAsErrors == null)
				{
					element.Add(new XElement(xmlns + "TreatWarningsAsErrors", "true"));
				}
				else
				{
					treatAsErrors.Value = "true";
				}

				var langVersion = element.Element(xmlns + "LangVersion");
				if (langVersion == null)
				{
					element.Add(new XElement(xmlns + "LangVersion", "6"));
				}
				else
				{
					langVersion.Value = "6";
				}

				var useVsHost = element.Element(xmlns + "UseVSHostingProcess");
				if (useVsHost == null)
				{
					element.Add(new XElement(xmlns + "UseVSHostingProcess", "false"));
				}
				else
				{
					useVsHost.Value = "false";
				}
			}
		}
		xdocument.Save(projectFile);
		CollapseEmptyElements(projectFile);
	}
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