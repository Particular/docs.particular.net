public static class PackageMetadataResourceExtensions
{
    public static async Task<DependencyInfo> GetPackageDetails(this NuGetSearcher searcher, string packageId)
    {
        var latestPackage = (await searcher.GetPackageAsync(packageId))
            .OrderByDescending(pkg => pkg.Identity.Version)
            .FirstOrDefault();

        if (latestPackage == null)
        {
            // Package is not yet on NuGet
            return null;
        }

        if (latestPackage.Authors == "Particular Software" || latestPackage.Authors == "NServiceBus Ltd")
        {
            return null;
        }

        return new DependencyInfo
        {
            Id = latestPackage.Identity.Id,
            RegistryUrl = $"https://www.nuget.org/packages/{latestPackage.Identity.Id}",
            License = latestPackage.LicenseMetadata?.License,
            LicenseUrl = latestPackage.LicenseUrl?.ToString(),
            ProjectUrl = latestPackage.ProjectUrl?.ToString()
        };
    }

    public static async Task<List<DependencyInfo>> GetDependencies(this NuGetSearcher searcher, string packageId)
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
                RegistryUrl = $"https://www.nuget.org/packages/{dependency.Id}",
                License = dependencyPackage.LicenseMetadata?.License,
                LicenseUrl = dependencyPackage.LicenseUrl?.ToString(),
                ProjectUrl = dependencyPackage.ProjectUrl?.ToString()
            });
        }

        return result;
    }
}