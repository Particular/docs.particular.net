using System.Collections.Concurrent;
using NuGet.Common;
using NuGet.Protocol.Core.Types;

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
}