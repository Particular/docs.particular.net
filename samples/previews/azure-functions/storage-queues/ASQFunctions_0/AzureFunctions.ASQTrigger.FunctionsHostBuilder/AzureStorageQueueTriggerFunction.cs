using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using NServiceBus;
using System.Threading.Tasks;

public class AzureStorageQueueTriggerFunction
{
    internal const string EndpointName = "ASQTriggerQueue";

    readonly FunctionEndpoint endpoint;

    #region endpoint-injection
    public AzureStorageQueueTriggerFunction(FunctionEndpoint endpoint)
    {
        this.endpoint = endpoint;
    }
    #endregion

    #region Function
    [FunctionName(EndpointName)]
    public async Task QueueTrigger(
        [QueueTrigger(EndpointName)]
        CloudQueueMessage message,
        ILogger logger,
        ExecutionContext context)
    {
        await endpoint.Process(message, context, logger);
    }
    #endregion
}