<Query Kind="Program" />

XNamespace xmlns = "http://schemas.microsoft.com/developer/msbuild/2003";
void Main()
{
	var toolsDiretory = Path.GetDirectoryName(Util.CurrentQueryPath);
	var docsDirectory = Directory.GetParent(toolsDiretory).FullName;
	foreach (var projectFile in Directory.EnumerateFiles(docsDirectory, "*.csproj", SearchOption.AllDirectories))
	{
		var xdocument = XDocument.Load(projectFile);
		var propertyGroups = xdocument.Descendants(xmlns + "PropertyGroup").ToList();

		foreach (var element in propertyGroups)
		{
			var condition = element.Attribute("Condition");
			if (condition != null)
			{
				if (
					condition.Value != " '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' " &&
					condition.Value != " '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ")
				{
					continue;
				}
				var treatAsErrors = element.Element(xmlns + "TreatWarningsAsErrors");
				if (treatAsErrors == null)
				{
					element.Add(new XElement(xmlns + "TreatWarningsAsErrors", "true"));
				}
				else
				{
					treatAsErrors.Value = "true";
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
	}
}