<Query Kind="Statements">
  <NuGetReference>NuGet.Core</NuGetReference>
  <Namespace>NuGet</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var location = Util.CurrentQuery.Location;
//var location = @"C:\Code\docs.particular.net\tools";
var nuGet = PackageRepositoryFactory.Default.CreateRepository("https://www.nuget.org/api/v2/");
var corePackageName = "NServiceBus";
var coreDependencies = Path.Combine(location, @"..\components\core-dependencies");
Directory.CreateDirectory(coreDependencies);
var filePaths = Directory.GetFiles(coreDependencies, "*.txt");
foreach (var filePath in filePaths)
{
    File.Delete(filePath);
}

var nugetAliasFile = Path.Combine(location, @"..\components\nugetAlias.txt");

Parallel.ForEach(File.ReadAllLines(nugetAliasFile), line =>
{
    var packageName = line.Split(':').Last().Trim();
    if (packageName == corePackageName)
    {
        return;
    }
	var packages = nuGet.FindPackagesById(packageName).Where(package => package.IsListed());
	var targetPath = Path.Combine(coreDependencies, $"{packageName}.txt");
	var processed = new List<SemanticVersion>();
	var minVersion = new SemanticVersion(3, 3, 0, 0);
	using (var writer = File.CreateText(targetPath))
	{
		foreach (var package in packages.OrderByDescending(x => x.Version))
		{
			var nsbDependency = package.DependencySets
				.SelectMany(x => x.Dependencies)
				.SingleOrDefault(d => d.Id == corePackageName);
			if (nsbDependency != null)
			{
				if (nsbDependency.VersionSpec.MinVersion < minVersion)
				{
					continue;
				}
				var semanticVersion = package.Version;
				if (processed.Any(_ => _.Version == semanticVersion.Version))
				{
					continue;
				}
				processed.Add(semanticVersion);
				writer.WriteLine(semanticVersion + " : " + nsbDependency.VersionSpec);
			}
		}
		writer.Flush();
	}
});