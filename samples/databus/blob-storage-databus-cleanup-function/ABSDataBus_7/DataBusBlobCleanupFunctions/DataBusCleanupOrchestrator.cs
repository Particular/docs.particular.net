#nullable disable

using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

public class DataBusCleanupOrchestrator(ILogger<DataBusCleanupOrchestrator> logger)
{
    #region DataBusCleanupOrchestratorFunction

    [Function(nameof(DataBusCleanupOrchestrator))]
    public async Task RunOrchestrator([OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var blobData = context.GetInput<DataBusBlobData>();

        logger.LogInformation("Orchestrating deletion for blob at {name} with ValidUntilUtc of {validUntilUtc}", blobData.Name, blobData.ValidUntilUtc);

        var validUntilUtc = DataBusBlobTimeoutCalculator.ToUtcDateTime(blobData.ValidUntilUtc);

        DateTime timeoutUntil;

        //Timeouts currently have a 7 day limit, use 6 day loops until the wait is less than 6 days
        do
        {
            timeoutUntil = validUntilUtc > context.CurrentUtcDateTime.AddDays(6) ? context.CurrentUtcDateTime.AddDays(6) : validUntilUtc;

            logger.LogInformation("Waiting until {timeoutUntil}/{validUntilUtc} for blob at {blobData.Name}. Currently {context.CurrentUtcDateTime}.", timeoutUntil, validUntilUtc, blobData.Name, context.CurrentUtcDateTime);

            await context.CreateTimer(DataBusBlobTimeoutCalculator.ToUtcDateTime(blobData.ValidUntilUtc), CancellationToken.None);
        } while (validUntilUtc > timeoutUntil);

        await context.CallActivityAsync("DeleteBlob", blobData);
    }

    #endregion
}