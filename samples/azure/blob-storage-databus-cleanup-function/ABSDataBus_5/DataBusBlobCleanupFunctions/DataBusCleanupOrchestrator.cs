using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

public static class DataBusCleanupOrchestrator
{
    #region DataBusCleanupOrchestratorFunction

    [FunctionName(nameof(DataBusCleanupOrchestrator))]
    public static async Task RunOrchestrator([OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
    {
        var blobData = context.GetInput<DataBusBlobData>();

        log.LogInformation($"Orchestrating deletion for blob at {blobData.Path} with ValidUntilUtc of {blobData.ValidUntilUtc}");

        var validUntilUtc = DataBusBlobTimeoutCalculator.ToUtcDateTime(blobData.ValidUntilUtc);

        DateTime timeoutUntil;

        //Timeouts currently have a 7 day limit, use 6 day loops until the wait is less than 6 days
        do
        {
            timeoutUntil = validUntilUtc > context.CurrentUtcDateTime.AddDays(6) ? context.CurrentUtcDateTime.AddDays(2) : validUntilUtc;
            log.LogInformation($"Waiting until {timeoutUntil}/{validUntilUtc} for blob at {blobData.Path}. Currently {context.CurrentUtcDateTime}.");
            await context.CreateTimer(DataBusBlobTimeoutCalculator.ToUtcDateTime(blobData.ValidUntilUtc), CancellationToken.None);
        } while (validUntilUtc > timeoutUntil);


        await context.CallActivityAsync("DeleteBlob", blobData);
    }

    #endregion
}