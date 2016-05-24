<Query Kind="Statements">
  <NuGetReference>NuGet.Core</NuGetReference>
  <Namespace>NuGet</Namespace>
</Query>

var nuGet = PackageRepositoryFactory.Default.CreateRepository("https://www.nuget.org/api/v2/");	
var corePackageName = "NServiceBus";
var nugetAliasFile = Path.Combine(Util.CurrentQuery.Location, @"..\components\nugetAlias.txt");

(

	from line in File.ReadLines(nugetAliasFile).AsParallel()
	let packageName = line.Split(':').Last().Trim()
	where packageName != corePackageName
	from package in nuGet.FindPackagesById(packageName)
	where package.IsListed()
	let dependencySet = package.DependencySets.FirstOrDefault()
	let nsbDependency = dependencySet?.Dependencies?.SingleOrDefault(d => d.Id == corePackageName)
	orderby package.Id ascending, package.Version descending
	group new 
	{ 
		PackageVersion = package.Version.ToString(), 
		CoreVersionRange = nsbDependency?.VersionSpec?.ToString(), 
		CoreMajor = nsbDependency?.VersionSpec?.MinVersion?.Version?.Major
	}
	by package.Id into g
	select g
	
).Dump();