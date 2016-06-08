<Query Kind="Statements">
  <NuGetReference>NuGet.Core</NuGetReference>
  <Namespace>NuGet</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var location = Util.CurrentQuery.Location;
//var location = @"C:\Code\docs.particular.net\tools";
var nuGet = PackageRepositoryFactory.Default.CreateRepository("https://www.nuget.org/api/v2/");
var corePackageName = "NServiceBus";
var minCoreVersion = new SemanticVersion(3, 3, 0, 0);
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
    using (var writer = File.CreateText(targetPath))
    {
        var processed = new List<Version>();
        foreach (var package in packages.OrderByDescending(x => x.Version))
        {
            var packageVersion = package.Version.Version;
            if (packageVersion.Major == 0)
            {
                continue;
			}
			var nsbDependency = package.DependencySets
				.SelectMany(x => x.Dependencies)
				.SingleOrDefault(d => d.Id == corePackageName);
			if (nsbDependency == null)
			{
				continue;
			}
			if (nsbDependency.VersionSpec.MinVersion < minCoreVersion)
			{
				continue;
			}

			var majorVersion = new Version(packageVersion.Major, packageVersion.Minor);

			if (processed.Any(_ => _ == majorVersion))
			{
				continue;
			}
			processed.Add(majorVersion);
			var minVersion = nsbDependency.VersionSpec.MinVersion.Version;
			writer.WriteLine($"{majorVersion} : {minVersion.Major}");
		}
		writer.Flush();
	}
});
