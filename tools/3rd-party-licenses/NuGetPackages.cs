using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using YamlDotNet.Serialization;
using System.Text.Json;
using NuGet.Commands;

class DotnetResult
{
    public Project[] Projects { get; set; }
}

class Project
{
    public string Path { get; set; }
    public Framework[] Frameworks { get; set; }
}

class Framework
{
    public CSProjPackage[] TopLevelPackages { get; set; }
}

class CSProjPackage
{
    public string Id { get; set; }
}

public class NuGetPackages(string componentsPath, Tuple<string, string>[] solutionFiles)
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

            await Runner.ExecuteCommand(".", "dotnet", $"restore {solutionFile}");
            var result = await Runner.ExecuteCommand(".", "dotnet", $"list {solutionFile} package --format json");

            var resultJson = JsonSerializer.Deserialize<DotnetResult>(result,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            foreach (var project in resultJson.Projects)
            {
                if (project.Path.Contains("test", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                Console.WriteLine($"Getting packages for {project.Path}");
                if (project.Frameworks == null)
                {
                    continue;
                }

                foreach (var projectFramework in project.Frameworks)
                {
                    foreach (var projectFrameworkTopLevelPackage in projectFramework.TopLevelPackages)
                    {
                        var packageDetails = await searcher.GetPackageDetails(projectFrameworkTopLevelPackage.Id);
                        if (packageDetails != null && !dependencies.ContainsKey(packageDetails.Id))
                        {
                            dependencies.Add(packageDetails.Id, packageDetails);
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