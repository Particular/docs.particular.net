using System;
using System.Threading.Tasks;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

public class DataBusBlobCreated
{
    private readonly DataBusBlobTimeoutCalculator calculator;

    public DataBusBlobCreated(DataBusBlobTimeoutCalculator calculator)
    {
        this.calculator = calculator;
    }

    #region DataBusBlobCreatedFunction

    [FunctionName(nameof(DataBusBlobCreated))]
    public async Task Run([BlobTrigger("databus/{name}", Connection = "DataBusStorageAccount")]CloudBlockBlob  myBlob, [DurableClient] IDurableOrchestrationClient starter, ILogger log)
    {
        log.LogInformation($"Blob created at {myBlob.Uri}");

        var instanceId = myBlob.Name;

        var existingInstance = await starter.GetStatusAsync(instanceId);
        if (existingInstance != null)
        {
            log.LogInformation($"{nameof(DataBusCleanupOrchestrator)} has already been started for blob {myBlob.Uri}.");
            return;
        }

        var validUntilUtc =  calculator.GetValidUntil(myBlob);

        if (validUntilUtc == DateTime.MaxValue)
        {
            log.LogError($"Could not parse the 'ValidUntil' value for blob {myBlob.Uri}. Cleanup will not happen on this blob. You may consider manually removing this entry if non-expiry is incorrect.");
            return;
        }

        await starter.StartNewAsync(nameof(DataBusCleanupOrchestrator), instanceId, new DataBusBlobData
        {
            Path = myBlob.Uri.ToString(),
            ValidUntilUtc = calculator.ToWireFormattedString(validUntilUtc)
        });
    }

    #endregion
}
