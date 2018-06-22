using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

public static class DataBusBlobCreated
{
    #region DataBusBlobCreatedFunction
    [FunctionName(nameof(DataBusBlobCreated))]
    public static async Task Run([BlobTrigger("databus/{name}", Connection = "DataBusStorageAccount")]CloudBlockBlob myBlob, [OrchestrationClient] DurableOrchestrationClient starter, TraceWriter log)
    {
        log.Info($"Blob created at {myBlob.Uri}");

        var instanceId = myBlob.Name;

        var existingInstance = await starter.GetStatusAsync(instanceId);
        if (existingInstance != null)
        {
            log.Info($"{nameof(DataBusCleanupOrchestrator)} has already been started for blob {myBlob.Uri}.");
            return;
        }

        var validUntilUtc = DataBusBlobTimeoutCalculator.GetValidUntil(myBlob);

        if (validUntilUtc == DateTime.MaxValue)
        {
            log.Error($"Could not parse the 'ValidUntil' value `{myBlob.Metadata["ValidUntil"]}` for blob {myBlob.Uri}. Cleanup will not happen on this blob. You may consider manually removing this entry if non-expiry is incorrect.");
            return;
        }

        await starter.StartNewAsync(nameof(DataBusCleanupOrchestrator), instanceId, new DataBusBlobData
        {
            Path = myBlob.Uri.ToString(),
            ValidUntilUtc = DataBusBlobTimeoutCalculator.ToWireFormattedString(validUntilUtc)
        });

    }
    #endregion
}
