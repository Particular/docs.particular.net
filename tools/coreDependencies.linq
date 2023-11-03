<Query Kind="Program">
  <NuGetReference>NuGet.Protocol</NuGetReference>
  <Namespace>NuGet</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>NuGet.Protocol.Core.Types</Namespace>
  <Namespace>NuGet.Protocol</Namespace>
  <Namespace>NuGet.Versioning</Namespace>
  <Namespace>NuGet.Common</Namespace>
  <Namespace>NuGet.Packaging.Core</Namespace>
</Query>

string corePackageName = "NServiceBus";
string location = Util.CurrentQuery.Location;
//string location = @"C:\Code\docs.particular.net\tools";

SourceRepository nuGet = Repository.Factory.GetCoreV3("https://www.nuget.org/api/v2/");
SourceRepository  feedz = Repository.Factory.GetCoreV3("https://f.feedz.io/particular-software/packages/nuget");
SemanticVersion minCoreVersion = new SemanticVersion(3, 3, 0);

async Task Main()
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

    var allPackages = await GetAllPackages(packageNames);

    foreach (var packageName in packageNames)
    {
        Process(allPackages, packageName, coreDependencies);
    }
}

void Process(List<IPackageSearchMetadata> allPackages, string packageName, string coreDependencies)
{
    var packagesForName = allPackages.Where(x => x.Identity.Id == packageName).ToList();
    var targetPath = Path.Combine(coreDependencies, $"{packageName}.txt");
    using (var writer = File.CreateText(targetPath))
    {
        var processed = new List<Version>();
        foreach (var package in packagesForName.OrderByDescending(x => x.Identity.Version))
        {
            var packageVersion = package.Identity.Version;
            var nsbDependency = GetDependencies(package, allPackages)
                .FirstOrDefault(d => d.Id == corePackageName);
            if (nsbDependency == null)
            {
                continue;
            }
            if (nsbDependency.VersionRange.MinVersion < minCoreVersion)
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
      if (nsbDependency.VersionRange.IsMaxInclusive)
      {
        version = nsbDependency.VersionRange.MaxVersion.Version.Major;
      }
      else if (nsbDependency.VersionRange.MaxVersion != null)
      {
        version = nsbDependency.VersionRange.MaxVersion.Version.Major - 1;
      }
      else
      {
        version = nsbDependency.VersionRange.MinVersion.Version.Major;
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

async Task<List<IPackageSearchMetadata>> GetAllPackages(List<string> packageNames)
{
    var allPackages = new ConcurrentBag<IPackageSearchMetadata>();

  await Parallel.ForEachAsync(packageNames, async (packageName, token) =>
  {
    var packages = await ListedPackages(nuGet, packageName);

    foreach (var package in packages)
    {
      allPackages.Add(package);
    }

    packages = await ListedPackages(feedz, packageName);

    foreach (var package in packages)
    {
      allPackages.Add(package);
    }
  });

    return allPackages.ToList();
}

static async Task<IEnumerable<IPackageSearchMetadata>> ListedPackages(SourceRepository repository, string packageName)
{
  var resource = await repository.GetResourceAsync<PackageMetadataResource>();
  var cache = new SourceCacheContext();
  var logger = NullLogger.Instance;

    return await resource.GetMetadataAsync(packageName, true, false, cache, logger, CancellationToken.None);
}

static IEnumerable<PackageDependency> GetDependencies(IPackageSearchMetadata package, List<IPackageSearchMetadata> packages)
{
  foreach (var dependency in package.DependencySets.SelectMany(x => x.Packages))
  {
    yield return dependency;

    foreach (var subPackage in packages.OrderBy(x => x.Identity.Version))
    {
      if (dependency.VersionRange.Satisfies(subPackage.Identity.Version))
      {
        if (subPackage.Identity.Id != dependency.Id || !dependency.VersionRange.Satisfies(subPackage.Identity.Version))
        {
          continue;
        }
        foreach (var subDependency in subPackage.DependencySets.SelectMany(x => x.Packages))
        {
          yield return subDependency;
        }
      }
    }
  }
}