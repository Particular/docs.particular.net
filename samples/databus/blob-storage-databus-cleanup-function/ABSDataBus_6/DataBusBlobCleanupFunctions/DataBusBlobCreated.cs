using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

public class DataBusBlobCreated(ILogger<DataBusBlobCreated> logger)
{
    #region DataBusBlobCreatedFunction

    [Function(nameof(DataBusBlobCreated))]
    public async Task Run([BlobTrigger("databus/{name}", Connection = "DataBusStorageAccount")] Stream blob, string name, Uri uri, IDictionary<string, string> metadata, [DurableClient] DurableTaskClient durableTaskClient, CancellationToken cancellationToken)
    {
        logger.LogInformation("Blob created at {uri}", uri);

        var instanceId = name;
        var existingInstance = await durableTaskClient.GetInstanceAsync(instanceId, cancellationToken);

        if (existingInstance != null)
        {
            logger.LogInformation("{DataBusCleanupOrchestratorName} has already been started for blob {uri}.", DataBusCleanupOrchestratorName, uri);
            return;
        }

        var validUntilUtc = DataBusBlobTimeoutCalculator.GetValidUntil(metadata);

        if (validUntilUtc == DateTime.MaxValue)
        {
            logger.LogError("Could not parse the 'ValidUntil' value for blob {uri}. Cleanup will not happen on this blob. You may consider manually removing this entry if non-expiry is incorrect.", uri);
            return;
        }

        await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(DataBusCleanupOrchestratorName, new DataBusBlobData
        {
            Name = name,
            ValidUntilUtc = DataBusBlobTimeoutCalculator.ToWireFormattedString(validUntilUtc)
        },
        new StartOrchestrationOptions()
        {
            InstanceId = instanceId
        }, cancellationToken);
    }

    #endregion

    static readonly string DataBusCleanupOrchestratorName = nameof(DataBusCleanupOrchestrator);
}