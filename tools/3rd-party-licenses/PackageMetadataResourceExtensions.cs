using NuGet.Common;
using NuGet.Protocol.Core.Types;

public static class PackageMetadataResourceExtensions
{
    private static SourceCacheContext sourceCacheContext = new SourceCacheContext
    {
        MaxAge = DateTimeOffset.UtcNow,
        NoCache = true,
    };

    public static async Task<List<DependencyInfo>> GetDependencies(this NuGetSearcher searcher, string packageId, ILogger logger)
    {
        Console.WriteLine("Getting dependencies for " + packageId);

        var result = new List<DependencyInfo>();

        var latestPackage = (await searcher.GetPackageAsync(packageId))
            .OrderByDescending(pkg => pkg.Identity.Version)
            .FirstOrDefault();

        if (latestPackage == null)
        {
            // Package is not yet on NuGet
            return result;
        }

        var dependencies = latestPackage.DependencySets.SelectMany(set => set.Packages).Distinct().ToArray();

        foreach (var dependency in dependencies)
        {
            if (dependency.Id == "NServiceBus") // Just an optimization
            {
                continue;
            }

            var dependencyPackage = (await searcher.GetPackageAsync(dependency.Id))
                .OrderByDescending(pkg => pkg.Identity.Version)
                .FirstOrDefault(pkg => dependency.VersionRange.Satisfies(pkg.Identity.Version));

            if (dependencyPackage.Authors == "Particular Software" || dependencyPackage.Authors == "NServiceBus Ltd")
            {
                continue;
            }

            result.Add(new DependencyInfo
            {
                Id = dependency.Id,
                License = dependencyPackage.LicenseMetadata,
                LicenseUrl = dependencyPackage.LicenseUrl?.ToString(),
                ProjectUrl = dependencyPackage.ProjectUrl?.ToString()
            });
        }

        return result;
    }
}