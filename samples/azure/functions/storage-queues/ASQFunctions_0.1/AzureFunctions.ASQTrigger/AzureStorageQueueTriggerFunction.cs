using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using NServiceBus;
using NServiceBus.AzureFunctions.StorageQueues;
using System.Threading.Tasks;

public class AzureStorageQueueTriggerFunction
{
    private const string EndpointName = "ASQTriggerQueue";

    #region Function

    [FunctionName(EndpointName)]
    public static async Task QueueTrigger(
        [QueueTrigger(EndpointName)]
        CloudQueueMessage message,
        ILogger log,
        ExecutionContext context)
    {
        await endpoint.Process(message, context);
    }

    #endregion

    #region EndpointSetup

    private static FunctionEndpoint endpoint = new FunctionEndpoint(executionContext =>
    {
        var configuration = new StorageQueueTriggeredEndpointConfiguration(EndpointName);

        configuration.UseSerialization<NewtonsoftSerializer>();

        return configuration;
    });

    #endregion
}