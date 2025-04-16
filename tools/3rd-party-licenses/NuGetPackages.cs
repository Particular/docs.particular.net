using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using YamlDotNet.Serialization;
using System.Xml.Linq;
using System.Xml.XPath;

public class NuGetPackages(string componentsPath, params (string name, string solutionFile)[] solutionFiles)
{
    const string Source = "https://api.nuget.org/v3/index.json";
    const string CorePackageId = "NServiceBus";

    private NuGetSearcher searcher;

    static IEnumerable<SerializationComponent> GetComponents(string path)
    {
        List<SerializationComponent> components;
        using (var reader = File.OpenText(path))
        {
            components = new Deserializer().Deserialize<List<SerializationComponent>>(reader);
        }

        return components
            .Where(component => component.UsesNuget && component.SupportLevel == SupportLevel.Regular);
    }

    public async Task<List<PackageWrapper>> GetPackagesForSolution()
    {
        var results = new List<PackageWrapper>();
        foreach (var (name, solutionFile) in solutionFiles)
        {
            var dependencies = new Dictionary<string, DependencyInfo>(StringComparer.OrdinalIgnoreCase);

            Console.WriteLine($"Getting packages for {solutionFile}");

            var solutionDirectory = Path.GetDirectoryName(solutionFile);
            var projectFiles = Directory.GetFiles(solutionDirectory, "*.csproj", SearchOption.AllDirectories)
                .Where(path => !Path.GetFileName(path).Contains("test", StringComparison.OrdinalIgnoreCase))
                .Where(path => !Path.GetFileName(path).Contains("PlatformSample", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            foreach (var project in projectFiles)
            {
                Console.WriteLine($"Getting packages for {project}");
                var xdoc = XDocument.Load(project);
                var topLevelPackages = xdoc.XPathSelectElements("/Project/ItemGroup/PackageReference")
                    .Where(pkgRef =>
                    {
                        var privateAssets = pkgRef.Attribute("PrivateAssets")?.Value;
                        if (privateAssets is not null && privateAssets.Equals("All", StringComparison.OrdinalIgnoreCase))
                        {
                            return false;
                        }

                        return true;
                    })
                    .Select(pkgRef => pkgRef.Attribute("Include")!.Value)
                    .ToArray();

                foreach (var projectFrameworkTopLevelPackageId in topLevelPackages)
                {
                    if (!dependencies.ContainsKey(projectFrameworkTopLevelPackageId))
                    {
                        var packageDetails = await searcher.GetPackageDetails(projectFrameworkTopLevelPackageId);
                        if (packageDetails is not null)
                        {
                            dependencies.Add(projectFrameworkTopLevelPackageId, packageDetails);
                        }
                    }
                }
            }

            var dependenciesList = dependencies.Values.OrderBy(p => p.Id).ToList();

            results.Add(new PackageWrapper($"{name} NuGet packages", dependenciesList));
        }

        return results;
    }

    public async Task Initialize()
    {
        var packageMetadata = await new SourceRepository(new PackageSource(Source), Repository.Provider.GetCoreV3())
            .GetResourceAsync<PackageMetadataResource>();
        searcher = new NuGetSearcher(packageMetadata, new Logger());
    }

    public async Task<List<PackageWrapper>> GetPackages()
    {
        var results = new [] {new PackageWrapper(CorePackageId, await searcher.GetDependencies(CorePackageId))}
            .Concat((await Task.WhenAll(GetComponents(componentsPath)
                .SelectMany(component => component.NugetOrder
                    .Where(packageId => packageId != CorePackageId)
                    .Distinct()
                    .Select(async packageId => new PackageWrapper(packageId, await searcher.GetDependencies(packageId)))
                ))))
            .ToList();

        return results;
    }
}