using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using YamlDotNet.Serialization;

var source = "https://api.nuget.org/v3/index.json";
var componentsPath = Path.Combine(@"../../../../../components/components.yaml");

var corePackageId = "NServiceBus";
var includePath = Path.Combine(@"../../../../../platform/third-party-license-data.include.md");

var logger = new Logger();

var packageMetadata = await new SourceRepository(new PackageSource(source), Repository.Provider.GetCoreV3()).GetResourceAsync<PackageMetadataResource>();
var searcher = new NuGetSearcher(packageMetadata, logger);

var corePackage = new Package
{
    Id = corePackageId,
    Category = ComponentCategory.Core,
    Dependencies = await searcher.GetDependencies(corePackageId, logger)
};

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

var downstreamPackages =
    (await Task.WhenAll(GetComponents(componentsPath)
        .SelectMany(component => component.NugetOrder
            .Where(packageId => packageId != corePackageId)
            .Select(packageId =>
                new
                {
                    Id = packageId,
                    Category = component.Category,
                }))
        .Distinct()
        .Select(async package =>
            new Package
            {
                Id = package.Id,
                Category = package.Category,
                Dependencies = await searcher.GetDependencies(package.Id, logger)
            })))
    .OrderBy(package => package.Id)
    .ToList();

await using var output = new StreamWriter(includePath, append: false);
output.WriteLine("| Library | License | Project Site |");
output.WriteLine("|:-----------|:-------:|:------------:|");
output.WritePackages(new List<Package>([corePackage]).Concat(downstreamPackages).DistinctBy(package => package.Id));