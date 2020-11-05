using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs.Extensions.Http;

public static class DataBusOrchestrateExistingBlobs
{
    #region DataBusOrchestrateExistingBlobsFunction

    [FunctionName(nameof(DataBusOrchestrateExistingBlobs))]
    public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, [DurableClient] IDurableOrchestrationClient starter, ILogger log)
    {
        try
        {
            var storageConnectionString = Environment.GetEnvironmentVariable("DataBusStorageAccount");
            if (!CloudStorageAccount.TryParse(storageConnectionString, out var storageAccount))
            {
                throw new InvalidOperationException("Invalid connection string.");
            }

            var cloudBlobClient = storageAccount.CreateCloudBlobClient();
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
                        log.LogInformation($"{nameof(DataBusCleanupOrchestrator)} has already been started for blob {blockBlob.Uri}.");
                        continue;
                    }

                    var validUntilUtc = DataBusBlobTimeoutCalculator.GetValidUntil(blockBlob);

                    if (validUntilUtc == DateTime.MaxValue)
                    {
                        log.LogError($"Could not parse the 'ValidUntilUtc' value for blob {blockBlob.Uri}. Cleanup will not happen on this blob. You may consider manually removing this entry if non-expiry is incorrect.");
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
        catch (Exception exception)
        {
            var result = new ObjectResult(exception.Message)
            {
                StatusCode = (int) HttpStatusCode.InternalServerError
            };

            return result;
        }

        return new OkObjectResult("DataBusOrchestrateExistingBlobs has completed.");
    }

    #endregion
}
