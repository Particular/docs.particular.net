using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

public static class DataBusOrchestrateExistingBlobs
{
    #region DataBusOrchestrateExistingBlobsFunction
    [FunctionName(nameof(DataBusOrchestrateExistingBlobs))]
    [NoAutomaticTrigger]
    public static async Task Run(
        [OrchestrationClient] DurableOrchestrationClient starter,
        TraceWriter log)
    {
        var storageConnectionString = Environment.GetEnvironmentVariable("DataBusStorageAccount");
        var cloudStorageAccount = CloudStorageAccount.Parse(storageConnectionString);
        var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        var container = cloudBlobClient.GetContainerReference("databus");

        BlobContinuationToken token = null;
        do
        {
            var segment = await container.ListBlobsSegmentedAsync(token).ConfigureAwait(false);

            token = segment.ContinuationToken;

            foreach (var blockBlob in segment.Results.Where(blob => blob is CloudBlockBlob).Cast<CloudBlockBlob>())
            {
                var instanceId = blockBlob.Name;

                var existingInstance = await starter.GetStatusAsync(instanceId);
                if (existingInstance != null)
                {
                    log.Info($"{nameof(DataBusCleanupOrchestrator)} has already been started for blob {blockBlob.Uri}.");
                    return;
                }

                var validUntilUtc = DataBusBlobTimeoutCalculator.GetValidUntil(blockBlob);

                if (validUntilUtc == DateTime.MaxValue)
                {
                    log.Error($"Could not parse the 'ValidUntil' value `{blockBlob.Metadata["ValidUntil"]}` for blob {blockBlob.Uri}. Cleanup will not happen on this blob. You may consider manually removing this entry if non-expiry is incorrect.");
                    continue;
                }

                await starter.StartNewAsync(nameof(DataBusCleanupOrchestrator), instanceId, new DataBusBlobData
                {
                    Path = blockBlob.Uri.ToString(),
                    ValidUntilUtc = DataBusBlobTimeoutCalculator.ToWireFormattedString(validUntilUtc)
                });
            }

        } while (token != null);
    }
    #endregion
}
