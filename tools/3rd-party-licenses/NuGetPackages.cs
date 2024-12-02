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

public class NuGetPackages(string componentsPath, string[] solutionFiles)
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

    public async Task<List<DependencyInfo>> GetPackagesForSolution()
    {
        var list = new List<DependencyInfo>();

        foreach (var solutionFile in solutionFiles)
        {
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
                        if (packageDetails != null)
                        {
                            list.Add(packageDetails);
                        }
                    }
                }
            }
        }

        return list;
    }

    public async Task Initialize()
    {
        var packageMetadata = await new SourceRepository(new PackageSource(Source), Repository.Provider.GetCoreV3())
            .GetResourceAsync<PackageMetadataResource>();
        searcher = new NuGetSearcher(packageMetadata, new Logger());
    }

    public async Task<List<DependencyInfo>> GetPackages()
    {
        var results = (await searcher.GetDependencies(CorePackageId))
            .Concat((await Task.WhenAll(GetComponents(componentsPath)
                .SelectMany(component => component.NugetOrder
                    .Where(packageId => packageId != CorePackageId)
                    .Distinct()
                    .Select(packageId => searcher.GetDependencies(packageId))
                ))).SelectMany(list => list))
            .ToList();

        return results;
    }
}