<Query Kind="Program">
  <NuGetReference>NuGet.Core</NuGetReference>
  <Namespace>NuGet</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

string corePackageName = "NServiceBus";
string location = Util.CurrentQuery.Location;
//string location = @"C:\Code\docs.particular.net\tools";

IPackageRepository nuGet = PackageRepositoryFactory.Default.CreateRepository("https://www.nuget.org/api/v2/");
IPackageRepository myget = PackageRepositoryFactory.Default.CreateRepository("https://www.myget.org/F/particular/");
SemanticVersion minCoreVersion = new SemanticVersion(3, 3, 0, 0);

void Main()
{
    var coreDependencies = Path.Combine(location, @"..\components\core-dependencies");
    Directory.CreateDirectory(coreDependencies);
    var filePaths = Directory.GetFiles(coreDependencies, "*.txt");
    foreach (var filePath in filePaths)
    {
        File.Delete(filePath);
    }

    var nugetAliasFile = Path.Combine(location, @"..\components\nugetAlias.txt");

    var packageNames = GetPackageNames(nugetAliasFile, corePackageName);

    var allPackages = GetAllPackages(packageNames);

    foreach (var packageName in packageNames)
    {
        Process(allPackages, packageName, coreDependencies);
    }
}

void Process(List<IPackage> allPackages, string packageName, string coreDependencies)
{
    var packagesForName = allPackages.Where(x => x.Id == packageName).ToList();
    var targetPath = Path.Combine(coreDependencies, $"{packageName}.txt");
    using (var writer = File.CreateText(targetPath))
    {
        var processed = new List<Version>();
        foreach (var package in packagesForName.OrderByDescending(x => x.Version))
        {
            var packageVersion = package.Version.Version;
            var nsbDependency = GetDependencies(package, allPackages)
                .FirstOrDefault(d => d.Id == corePackageName);
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
			int version;
			if (nsbDependency.VersionSpec.IsMaxInclusive)
			{
				version = nsbDependency.VersionSpec.MaxVersion.Version.Major;
			}
			else
			{
				version = nsbDependency.VersionSpec.MaxVersion.Version.Major - 1;
			}
			writer.WriteLine($"{majorVersion} : {version}");
        }
        writer.Flush();
    }
}

static List<string> GetPackageNames(string nugetAliasFile, string corePackageName)
{
    var packageNames = new List<string>();
    foreach (var line in File.ReadAllLines(nugetAliasFile))
    {
        var packageName = line.Split(':').Last().Trim();
        if (packageName == corePackageName)
        {
            continue;
        }
        packageNames.Add(packageName);
    }
    return packageNames;
}

List<IPackage> GetAllPackages(List<string> packageNames)
{
    var allPackages = new ConcurrentBag<IPackage>();
    Parallel.ForEach(packageNames, packageName =>
    {
        foreach (var package in ListedPackages(nuGet, packageName))
        {
            allPackages.Add(package);
        }
        foreach (var package in ListedPackages(myget, packageName))
        {
            allPackages.Add(package);
        }
    });
    return allPackages.ToList();
}

static IEnumerable<IPackage> ListedPackages(IPackageRepository nuGet, string packageName)
{
    return nuGet.FindPackagesById(packageName)
        .Where(package => package.IsListed());
}

static IEnumerable<PackageDependency> GetDependencies(IPackage package, List<IPackage> packages)
{
	foreach (var dependency in package.DependencySets.SelectMany(x => x.Dependencies))
	{
		yield return dependency;
		foreach (var subPackage in packages.OrderBy(x => x.Version))
		{
			if (subPackage.Id != dependency.Id || !dependency.VersionSpec.Satisfies(subPackage.Version))
			{
				continue;
			}
			foreach (var subDependency in subPackage.DependencySets.SelectMany(x => x.Dependencies))
			{
				yield return subDependency;
			}
		}
	}

}