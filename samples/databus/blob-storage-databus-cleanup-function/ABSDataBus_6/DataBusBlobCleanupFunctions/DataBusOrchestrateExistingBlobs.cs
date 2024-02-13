using System.Net;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

public class DataBusOrchestrateExistingBlobs(BlobContainerClient blobContainerClient, ILogger<DataBusOrchestrateExistingBlobs> logger)
{
    #region DataBusOrchestrateExistingBlobsFunction

    [Function(nameof(DataBusOrchestrateExistingBlobs))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req, [DurableClient] DurableTaskClient durableTaskClient, CancellationToken cancellationToken)
    {
        var counter = 0;

        try
        {
            var segment = blobContainerClient.GetBlobsAsync(traits: BlobTraits.Metadata, cancellationToken: cancellationToken).AsPages();

            await foreach (var blobPage in segment)
            {
                foreach (var blobItem in blobPage.Values)
                {
                    var instanceId = blobItem.Name;

                    var existingInstance = await durableTaskClient.GetInstanceAsync(instanceId, cancellationToken);

                    if (existingInstance != null)
                    {
                        logger.LogInformation("{name} has already been started for blob {blobItemName}.", nameof(DataBusCleanupOrchestrator), blobItem.Name);
                        continue;
                    }

                    var validUntilUtc = DataBusBlobTimeoutCalculator.GetValidUntil(blobItem.Metadata);

                    if (validUntilUtc == DateTime.MaxValue)
                    {
                        logger.LogError("Could not parse the 'ValidUntilUtc' value for blob {name}. Cleanup will not happen on this blob. You may consider manually removing this entry if non-expiry is incorrect.", blobItem.Name);
                        continue;
                    }

                    await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(nameof(DataBusCleanupOrchestrator), new DataBusBlobData
                    {
                        Name = blobItem.Name,
                        ValidUntilUtc = DataBusBlobTimeoutCalculator.ToWireFormattedString(validUntilUtc)
                    },
                    new StartOrchestrationOptions()
                    {
                        InstanceId = instanceId
                    }, cancellationToken);

                    counter++;
                }
            }
        }
        catch (Exception exception)
        {
            var result = new ObjectResult(exception.Message)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            return result;
        }

        var message = "DataBusOrchestrateExistingBlobs has completed." + (counter > 0 ? $" {counter} blob{(counter > 1 ? "s" : string.Empty)} will be tracked for clean-up." : string.Empty);

        return new OkObjectResult(message);
    }

    #endregion
}