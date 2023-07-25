﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

public class DataBusOrchestrateExistingBlobs
{
    readonly CloudBlobContainer container;

    public DataBusOrchestrateExistingBlobs(CloudBlobContainer container)
    {
        this.container = container;
    }

    #region DataBusOrchestrateExistingBlobsFunction

    [FunctionName(nameof(DataBusOrchestrateExistingBlobs))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, [DurableClient] IDurableOrchestrationClient starter, ILogger log)
    {
        var counter = 0;

        try
        {
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

                    counter++;
                }

            } while (token != null);
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
