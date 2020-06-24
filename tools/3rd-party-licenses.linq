<Query Kind="Program">
  <NuGetReference>NuGet.PackageManagement</NuGetReference>
  <NuGetReference>YamlDotNet</NuGetReference>
  <Namespace>NuGet.Common</Namespace>
  <Namespace>NuGet.Configuration</Namespace>
  <Namespace>NuGet.Packaging.Core</Namespace>
  <Namespace>NuGet.Protocol</Namespace>
  <Namespace>NuGet.Protocol.Core.Types</Namespace>
  <Namespace>NuGet.Versioning</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>YamlDotNet.Serialization</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
</Query>

async Task Main()
{
    var endOfLifePackages = new Dictionary<string, string>
    {
        { "NServiceBus.Azure", "This package has been split into NServiceBus.DataBus.AzureBlobStorage and NServiceBus.Persistence.AzureStorage." }
    };

    var source = "https://api.nuget.org/v3/index.json";
    var mygetSource = "https://www.myget.org/F/particular/api/v3/index.json";
    var componentsPath = Path.Combine(Util.CurrentQuery.Location, @"..\components\components.yaml");

    var corePackageId = "NServiceBus";
    var serviceControlPackageId = "Particular.PlatformSample.ServiceControl";
    var includePath = Path.Combine(Util.CurrentQuery.Location, @"..\nservicebus\upgrades\third-party-license-data.include.md");

    var logger = new Logger();

    var packageMetadata = await new SourceRepository(new PackageSource(source), Repository.Provider.GetCoreV3()).GetResourceAsync<PackageMetadataResource>();
    var searcher = new NuGetSearcher(packageMetadata, logger);

    var corePackage = new Package
    {
        Id = corePackageId,
        Category = ComponentCategory.Core,
        Dependencies = await searcher.GetDependencies(corePackageId, logger, endOfLifePackages)
    };

    var mygetPackageMetadata = await new SourceRepository(new PackageSource(mygetSource), Repository.Provider.GetCoreV3()).GetResourceAsync<PackageMetadataResource>();
    var mygetSearcher = new NuGetSearcher(mygetPackageMetadata, logger);

    var serviceControlPackage = new Package
    {
        Id = serviceControlPackageId,
        Category = ComponentCategory.Other,
        Dependencies = await mygetSearcher.GetDependencies(serviceControlPackageId, logger, endOfLifePackages)
    };

    var downstreamPackages =
        (await Task.WhenAll(GetComponents(componentsPath, corePackageId)
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
                    Dependencies = await searcher.GetDependencies(package.Id, logger, endOfLifePackages)
                })))
        .OrderBy(package => package.Id)
        .ToList();

//    corePackage.Dump();
//    foreach (var package in downstreamPackages)
//    {
//        package.Dump();
//    }
//    serviceControlPackage.Dump();

    using (var output = new StreamWriter(includePath, append: false))
    {
        output.WritePackage(corePackage);
        output.WritePackages(downstreamPackages, endOfLifePackages);
        output.WriteServiceControl(serviceControlPackage);
    }
}

public static class TextWriterExtensions
{
    public static void WritePackage(this TextWriter output, Package package) =>
        output.Write(
            package,
            () =>
            {
                output.WriteLine($"## [{package.Id}](/nuget/{package.Id})");
                output.WriteLine();
            });
            
    public static void WriteServiceControl(this TextWriter output, Package package) =>
        output.Write(
            package,
            () =>
            {
                output.WriteLine($"## ServiceControl");
                output.WriteLine();
            });            

    public static void WritePackages(this TextWriter output, IEnumerable<Package> packages, Dictionary<string, string> endOfLifePackages)
    {
        foreach (ComponentCategory category in Enum.GetValues(typeof(ComponentCategory)))
        {
            var categoryHeadingWritten = false;

            foreach (var package in packages.Where(package => package.Category == category))
            {
                var packageHeadingWritten = false;

                output.Write(
                    package,
                    () =>
                    {
                        if (!categoryHeadingWritten)
                        {
                            output.WriteLine($"## {category} packages");
                            output.WriteLine();
                            categoryHeadingWritten = true;
                        }
                    },
                    () =>
                    {
                        if (!packageHeadingWritten)
                        {
                            output.WriteLine($"### [{package.Id}](/nuget/{package.Id})");
                            output.WriteLine();

                            if (endOfLifePackages.TryGetValue(package.Id, out var reason))
                            {
                                output.WriteLine($"_{reason}_");
                                output.WriteLine();
                            }

                            packageHeadingWritten = true;
                        }
                    });
            }
        }
    }
    private static void Write(this TextWriter output, Package package, params Action[] writeHeadings)
    {
        if (!package.Dependencies.Any())
        {
            return;
        }

        foreach (var writeHeading in writeHeadings)
        {
            writeHeading();
        }

        output.WriteLine("| Depencency | Version | License | Project Site |");
        output.WriteLine("|:-----------|:-------:|:-------:|:------------:|");

        foreach (var dependency in package.Dependencies)
        {
            if(dependency.ProjectUrl == null)
            {
                package.Dump();
            }
            output.Write($"| [{dependency.Id}](https://www.nuget.org/packages/{dependency.Id}/) | {dependency.Version} | ");
            if(dependency.LicenseUrl != null)
			{
				output.Write($"[View License]({dependency.LicenseUrl})");
			}
            else
            {
                output.Write("<i title=\"The NuGet package contains no license information. Try checking the project site instead.\">None provided</i>");
            }
			output.Write(" | ");
			if (dependency.ProjectUrl != null)
			{
				output.Write($"[Project Site]({dependency.ProjectUrl})");
			}
            else
            {
                output.Write("<i>None provided</i>");
            }
            output.WriteLine(" |");
        }

        output.WriteLine();
    }
}

public static class PackageMetadataResourceExtensions
{
    private static SourceCacheContext sourceCacheContext = new SourceCacheContext
    {
        MaxAge = DateTimeOffset.UtcNow,
        NoCache = true,
    };

    public static async Task<List<DependencyInfo>> GetDependencies(
        this NuGetSearcher searcher, string packageId, ILogger logger, Dictionary<string, string> endOfLifePackages)
    {
        var latestPackage = (await searcher.GetPackageAsync(packageId))
            .OrderByDescending(pkg => pkg.Identity.Version)
            .FirstOrDefault();

        var dependencies = latestPackage.DependencySets.SelectMany(set => set.Packages).Distinct().ToArray();
        var result = new List<DependencyInfo>();

        foreach(var dependency in dependencies)
        {
            if(dependency.Id == "NServiceBus") // Just an optimization
            {
                continue;
            }
            
            var dependencyPackage = (await searcher.GetPackageAsync(dependency.Id))
                .OrderByDescending(pkg => pkg.Identity.Version)
                .FirstOrDefault(pkg => dependency.VersionRange.Satisfies(pkg.Identity.Version));

            if(dependencyPackage.Authors == "Particular Software" || dependencyPackage.Authors == "NServiceBus Ltd")
            {
                continue;
            }
                
            result.Add(new DependencyInfo
            {
                Id = dependency.Id,
				Version = dependencyPackage.Identity.Version,
				LicenseUrl = dependencyPackage.LicenseUrl?.ToString(),
                ProjectUrl = dependencyPackage.ProjectUrl?.ToString()
            });
        }

        return result;
    }
}

public static class PackageExtensions
{
    public static void Dump(this Package package, DateTimeOffset utcTomorrow)
    {
        package.Id.Dump("Package");

        package.Dependencies
            .OrderBy(d => d.Id)
            .Dump("Dependencies");
    }
}

public static class PackageSearchMetadataExtensions
{
    public static string ToMinorString(this IPackageSearchMetadata package) =>
        package == null ? null : $"{package.Identity.Id} {package.Identity.Version.ToMinorString()}";
}

public static class NugetVersionExtensions
{
    public static string ToMinorString(this NuGetVersion version) => $"{version.Major}.{version.Minor}.x";
}

static IEnumerable<SerializationComponent> GetComponents(string path, string corePackageId)
{
    List<SerializationComponent> components;
    using (var reader = File.OpenText(path))
    {
        components = new Deserializer().Deserialize<List<SerializationComponent>>(reader);
    }

    return components
        .Where(component => component.UsesNuget && component.SupportLevel == SupportLevel.Regular);
}

public class NuGetSearcher
{
    PackageMetadataResource resource;
    SourceCacheContext cacheContext;
    ILogger logger;
    ConcurrentDictionary<string, IEnumerable<IPackageSearchMetadata>> dictionary;

    public NuGetSearcher(PackageMetadataResource resource, ILogger logger)
    {
        this.resource = resource;
        this.logger = logger;
        cacheContext = new SourceCacheContext
        {
            MaxAge = DateTime.UtcNow,
            NoCache = true
        };
        dictionary = new ConcurrentDictionary<string, IEnumerable<IPackageSearchMetadata>>();
    }

    public async Task<IEnumerable<IPackageSearchMetadata>> GetPackageAsync(string packageId)
    {
        if (!dictionary.TryGetValue(packageId, out var metadata))
        {
            metadata = await resource.GetMetadataAsync(packageId, false, false, cacheContext, logger, CancellationToken.None);
            dictionary.TryAdd(packageId, metadata);
        }
        return metadata;
    }

    public IEnumerable<PackageDependency> GetDependencies(IPackageSearchMetadata package)
    {
        foreach (var dependency in package.DependencySets.SelectMany(x => x.Packages))
        {
            yield return dependency;
            if (dictionary.TryGetValue(dependency.Id, out var dependencyPackageList))
            {
                foreach (var subPackage in dependencyPackageList.OrderBy(x => x.Identity.Version))
                {
                    if (dependency.VersionRange.Satisfies(subPackage.Identity.Version))
                    {
                        foreach (var subDependency in GetDependencies(subPackage))
                        {
                            yield return subDependency;
                        }
                    }
                }
            }
        }
    }
}

public class Package
{
    public string Id { get; set; }
    public ComponentCategory Category { get; set; }
    public List<DependencyInfo> Dependencies { get; set; }
}

public class DependencyInfo
{
    public string Id { get; set; }
    public NuGetVersion Version { get; set; }
    public string LicenseUrl { get; set; }
    public string ProjectUrl { get; set; }
}

public class SerializationComponent
{
    public SerializationComponent()
    {
        this.UsesNuget = true;
        this.SupportLevel = SupportLevel.Regular;
        this.Category = ComponentCategory.Other;
        this.NugetOrder = new List<string>();
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public string Key { get; set; }
    public string DocsUrl { get; set; }
    public string ProjectUrl { get; set; }
    public string LicenseUrl { get; set; }
    public string GitHubOwner { get; set; }
    public bool UsesNuget { get; set; }
    public SupportLevel SupportLevel { get; set; }
    public ComponentCategory Category { get; set; }
    public List<string> NugetOrder { get; set; }
}

public enum SupportLevel
{
    Regular,
    Labs,
    Community,
}

public enum ComponentCategory
{
    Core,
    Transport,
    Persistence,
    Serializer,
    DependencyInjection,
    Logger,
    Databus,
    Host,
    Other,
}

public class Logger : ILogger
{
    public void LogDebug(string data) => Log(LogLevel.Debug, data);

    public void LogVerbose(string data) => Log(LogLevel.Verbose, data);

    public void LogInformation(string data) => Log(LogLevel.Information, data);

    public void LogMinimal(string data) => Log(LogLevel.Minimal, data);

    public void LogWarning(string data) => Log(LogLevel.Warning, data);

    public void LogError(string data) => Log(LogLevel.Error, data);

    public void LogInformationSummary(string data) => Log(LogLevel.Information, data);

    public void LogErrorSummary(string data) => Log(LogLevel.Error, data);

    public void Log(LogLevel level, string data)
    {
        switch (level)
        {
            case LogLevel.Minimal:
            case LogLevel.Warning:
            case LogLevel.Error:
                $"{level}:{data}".Dump();
                break;
            default:
                break;
        }
    }

    public Task LogAsync(LogLevel level, string data)
    {
        Log(level, data);
        return Task.CompletedTask;
    }

    public void Log(ILogMessage message) => Log(message.Level, message.Message);

    public Task LogAsync(ILogMessage message) => LogAsync(message.Level, message.Message);
}