<Query Kind="Program">
  <NuGetReference>NuGet.PackageManagement</NuGetReference>
  <NuGetReference>NuGet.Protocol.Core.v3</NuGetReference>
  <NuGetReference>YamlDotNet</NuGetReference>
  <Namespace>NuGet.Common</Namespace>
  <Namespace>NuGet.Configuration</Namespace>
  <Namespace>NuGet.Packaging.Core</Namespace>
  <Namespace>NuGet.Protocol</Namespace>
  <Namespace>NuGet.Protocol.Core.Types</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>YamlDotNet.Serialization</Namespace>
  <Namespace>NuGet.Versioning</Namespace>
</Query>

async Task Main()
{
    var source = "https://api.nuget.org/v3/index.json";
    var componentsPath = Path.Combine(Util.CurrentQuery.Location, @"..\components\components.yaml");

    var corePackageId = "NServiceBus";

    var coreMajorOverlapYears = 2;
    var coreMinorOverlapMonths = 6;
    var coreMonthsToShowUnsupportedVersions = 12;

    var downstreamMajorOverlapYears = 1;
    var downstreamMinorOverlapMonths = 3;
    var downstreamMonthsToShowUnsupportedVersions = 6;

    var corePath = Path.Combine(Util.CurrentQuery.Location, @"..\nservicebus\upgrades\supported-versions-nservicebus.include.md");
    var downstreamsPath = Path.Combine(Util.CurrentQuery.Location, @"..\nservicebus\upgrades\supported-versions-downstreams.include.md");
    var allVersionsPath = Path.Combine(Util.CurrentQuery.Location, @"..\nservicebus\upgrades\all-versions.include.md");

    var utcTomorrow = DateTime.UtcNow.Date.AddDays(1);
    var logger = new Logger();

    var packageMetadata = await new SourceRepository(new PackageSource(source), Repository.Provider.GetCoreV3()).GetResourceAsync<PackageMetadataResource>();

    var corePackage = new Package
    {
        Id = corePackageId,
        Category = ComponentCategory.Core,
        Versions = await packageMetadata.GetVersions(corePackageId, logger, coreMajorOverlapYears, coreMinorOverlapMonths, new List<Version>())
    };

    corePackage.Dump(utcTomorrow);

    var downstreamPackages =
        (await Task.WhenAll(GetComponents(componentsPath, corePackageId).Select(async package =>
           new Package
           {
               Id = package.NugetOrder.First(),
               Category = package.Category,
               Versions = await packageMetadata.GetVersions(package.NugetOrder.First(), logger, downstreamMajorOverlapYears, downstreamMinorOverlapMonths, corePackage.Versions)
           })))
        .ToList();

    foreach (var package in downstreamPackages)
    {
        package.Dump(utcTomorrow);
    }

    using (var output = new StreamWriter(corePath, append: false))
    {
        output.Write(corePackage, utcTomorrow, utcTomorrow.AddMonths(-coreMonthsToShowUnsupportedVersions), true);
    }

    using (var output = new StreamWriter(downstreamsPath, append: false))
    {
        output.Write(downstreamPackages, utcTomorrow, utcTomorrow.AddMonths(-downstreamMonthsToShowUnsupportedVersions), false);
    }

    using (var output = new StreamWriter(allVersionsPath, append: false))
    {
        output.Write(corePackage, utcTomorrow, null, true);
        output.Write(downstreamPackages, utcTomorrow, null, true);
    }
}

public static class TextWriterExtensions
{
    public static void Write(this TextWriter output, Package package, DateTimeOffset utcTomorrow, DateTimeOffset? earliest, bool force) =>
        output.Write(
            package.Versions,
            utcTomorrow,
            earliest,
            force,
            () =>
            {
                output.WriteLine($"### {package.Id}");
                output.WriteLine();
            });

    public static void Write(this TextWriter output, IEnumerable<Package> packages, DateTimeOffset utcTomorrow, DateTimeOffset? earliest, bool force)
    {
        foreach (ComponentCategory category in Enum.GetValues(typeof(ComponentCategory)))
        {
            var categoryHeadingWritten = false;

            foreach (var package in packages.Where(package => package.Category == category))
            {
                var packageHeadingWritten = false;

                output.Write(
                    package.Versions,
                    utcTomorrow,
                    earliest,
                    force,
                    () =>
                    {
                        if (!categoryHeadingWritten)
                        {
                            output.WriteLine($"### {category} packages");
                            output.WriteLine();
                            categoryHeadingWritten = true;
                        }
                    },
                    () =>
                    {
                        if (!packageHeadingWritten)
                        {
                            output.WriteLine($"#### {package.Id}");
                            output.WriteLine();
                            packageHeadingWritten = true;
                        }
                    });
            }
        }
    }

    private static void Write(this TextWriter output, List<Version> versions, DateTimeOffset utcTomorrow, DateTimeOffset? earliest, bool force, params Action[] writeHeadings)
    {
        var relevantVersions = versions
            .Where(version => !earliest.HasValue || (!version.PatchingEnd.HasValue || version.PatchingEnd.Value >= earliest.Value))
            .ToList();

        if (!force && !relevantVersions.Any())
        {
            return;
        }

        foreach (var writeHeading in writeHeadings)
        {
            writeHeading();
        }

        if (!relevantVersions.Any())
        {
            output.WriteLine($"No versions released{(earliest.HasValue ? $" since {earliest.Value.ToString("yyyy-MMM-dd")}" : "")}.");
            output.WriteLine();

            return;
        }

        output.WriteLine("| Version   | Released       | Supported until   | Explanation                       |");
        output.WriteLine("|:---------:|:--------------:|:-----------------:|:---------------------------------:|");

        foreach (var version in relevantVersions.OrderByDescending(version => version.First.Identity.Version))
        {
            var isSupported = !version.PatchingEnd.HasValue || version.PatchingEnd.Value > utcTomorrow;
            var open = isSupported ? "" : "~~";
            var close = isSupported ? "" : "~~";

            output.Write($"| ");
            output.Write($"{open}{version.First.Identity.Version.ToMinorString()}{close}".PadRight(9));
            output.Write($" | ");
            output.Write($"{open}{version.First.Published.Value.UtcDateTime.Date.ToString("yyyy-MM-dd")}{close}".PadRight(14));
            output.Write($" | ");
            output.Write($"{open}{version.PatchingEnd?.ToString("yyyy-MM-dd") ?? "-"}{close}".PadRight(17));
            output.Write($" | ");
            output.Write($"{open}{(version.PatchingEnd.HasValue ? version.PatchingEndReason : "-")}{close}".PadRight(33));
            output.WriteLine(" |");
        }

        output.WriteLine();
    }
}

public static class PackageMetadataResourceExtensions
{
    public static async Task<List<Version>> GetVersions(
        this PackageMetadataResource resource, string packageId, ILogger logger, int majorOverlapYears, int minorOverlapMonths, List<Version> upstreamVersions)
    {
        var minors = (await resource.GetMetadataAsync(packageId, false, false, logger, CancellationToken.None))
            .OrderBy(package => package.Identity.Version)
            .GroupBy(package => new { package.Identity.Version.Major, package.Identity.Version.Minor })
            .Select(group => new { First = group.First(), Last = group.Last(), })
            .ToList();

        var missingPublishedDate = minors.Where(package => !package.First.Published.HasValue).ToList();

        if (missingPublishedDate.Any())
        {
            throw new Exception($"These {packageId} packages have no published date: {string.Join(", ", missingPublishedDate.Select(minor => minor.First.Identity.Version))}");
        }

        return minors
            .Select(minor =>
            {
                var latestUpstreamsWithPatchingEnd = minor.Last.DependencySets
                    .SelectMany(set => set.Packages)
                    .Select(dep => upstreamVersions.LastOrDefault(version =>
                        version.Last.Identity.Id == dep.Id && dep.VersionRange.Satisfies(version.Last.Identity.Version)))
                    .Where(version => version != null && version.PatchingEnd.HasValue)
                    .OrderBy(version => version.PatchingEnd)
                    .ToList();

                var lastUpstreamToEndPatching = latestUpstreamsWithPatchingEnd.LastOrDefault();

                var lastMinorToSupportLastUpstreamToEndPatching = lastUpstreamToEndPatching == null
                    ? null
                    : minors.LastOrDefault(candidate =>
                        candidate.Last.DependencySets.Any(set =>
                            set.Packages.Any(dep =>
                                dep.Id == lastUpstreamToEndPatching.Last.Identity.Id &&
                                dep.VersionRange.Satisfies(lastUpstreamToEndPatching.Last.Identity.Version))));

                var nextMajor = minors
                    .GroupBy(candidate => candidate.First.Identity.Version.Major)
                    .Select(group => new { Package = group.First().First, ImpliedPatchingEnd = group.First().First.Published.Value.UtcDateTime.Date.AddYears(majorOverlapYears) })
                    .FirstOrDefault(comparand => comparand.Package.Identity.Version.Major > minor.First.Identity.Version.Major);

                var nextMinor = minors
                    .Select(candidate => new { Package = candidate.First, ImpliedPatchingEnd = candidate.First.Published.Value.UtcDateTime.Date.AddMonths(minorOverlapMonths) })
                    .FirstOrDefault(comparand =>
                        comparand.Package.Identity.Version.Major == minor.Last.Identity.Version.Major &&
                        comparand.Package.Identity.Version.Minor > minor.Last.Identity.Version.Minor);

                DateTime? patchingEnd = null;
                string patchingEndReason = null;

                var boundedBy = latestUpstreamsWithPatchingEnd.FirstOrDefault();
                var extendedBy = lastMinorToSupportLastUpstreamToEndPatching?.Last.Identity.Version == minor.Last.Identity.Version
                    ? lastUpstreamToEndPatching
                    : null;

                if (nextMajor != null && (!patchingEnd.HasValue || nextMajor.ImpliedPatchingEnd <= patchingEnd.Value))
                {
                    patchingEnd = nextMajor.ImpliedPatchingEnd;
                    patchingEndReason = $"Superseded by {nextMajor.Package.Identity.Version.ToMinorString()}";
                }

                if (nextMinor != null && (!patchingEnd.HasValue || nextMinor.ImpliedPatchingEnd <= patchingEnd.Value))
                {
                    patchingEnd = nextMinor.ImpliedPatchingEnd;
                    patchingEndReason = $"Superseded by {nextMinor.Package.Identity.Version.ToMinorString()}";
                }

                if (extendedBy != null && patchingEnd.HasValue && extendedBy.PatchingEnd.Value.Date > patchingEnd.Value.Date)
                {
                    patchingEnd = extendedBy.PatchingEnd;
                    patchingEndReason = patchingEnd.HasValue ? $"Extended by {extendedBy.ToString()}" : null;
                }

                if (boundedBy != null && (!patchingEnd.HasValue || boundedBy.PatchingEnd.Value.Date < patchingEnd.Value.Date))
                {
                    patchingEnd = boundedBy.PatchingEnd;
                    patchingEndReason = $"Bounded by {boundedBy.ToString()}";
                }

                return new Version
                {
                    First = minor.First,
                    Last = minor.Last,
                    BoundedBy = boundedBy,
                    ExtendedBy = extendedBy,
                    PatchingEnd = patchingEnd,
                    PatchingEndReason = patchingEndReason,
                };
            })
            .OrderBy(version => version.First.Identity)
            .ToList();
    }
}

public static class PackageExtensions
{
    public static void Dump(this Package package, DateTimeOffset utcTomorrow)
    {
        package.Id.Dump("Package");

        package.Versions
           .OrderByDescending(version => version.First.Identity.Version.Major)
           .ThenByDescending(version => version.First.Identity.Version.Minor)
           .Select(version => new
           {
               Package = version.ToString(),
               Published = version.First.Published?.UtcDateTime.Date.ToString("yyyy-MM-dd"),
               BoundedBy = version.BoundedBy?.ToString(),
               ExtendedBy = version.ExtendedBy?.ToString(),
               PatchingEnd = version.PatchingEnd?.ToString("yyyy-MM-dd"),
               PatchingEndReason = version.PatchingEndReason,
               CurrentlyPatched = !version.PatchingEnd.HasValue || version.PatchingEnd.Value > utcTomorrow
           })
           .Dump("Versions");
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
        .Where(component => component.UsesNuget && component.SupportLevel == SupportLevel.Regular)
        .Where(component => (component.NugetOrder.FirstOrDefault() ?? corePackageId) != corePackageId)
        .GroupBy(package => package.NugetOrder.First())
        .Select(group => group.First())
        .OrderBy(package => package.NugetOrder.First());
}

public class Package
{
    public string Id { get; set; }
    public ComponentCategory Category { get; set; }
    public List<Version> Versions { get; set; }
}

public class Version
{
    public IPackageSearchMetadata First { get; set; }
    public IPackageSearchMetadata Last { get; set; }
    public Version BoundedBy { get; set; }
    public Version ExtendedBy { get; set; }
    public DateTime? PatchingEnd { get; set; }
    public string PatchingEndReason { get; set; }
    public override string ToString() => this.First.ToMinorString();
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
    public string DocoUrl { get; set; }
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
    Container,
    Logger,
    Databus,
    Host,
    Other,
}

public class Logger : ILogger
{
    public void LogDebug(string data) { }
    public void LogVerbose(string data) { }
    public void LogInformation(string data) { }
    public void LogMinimal(string data) => $"INFO: {data}".Dump();
    public void LogWarning(string data) => $"WARNING: {data}".Dump();
    public void LogError(string data) => $"ERROR: {data}".Dump();
    public void LogInformationSummary(string data) => data.Dump("Information summary");
    public void LogErrorSummary(string data) => data.Dump("Error summary");
}