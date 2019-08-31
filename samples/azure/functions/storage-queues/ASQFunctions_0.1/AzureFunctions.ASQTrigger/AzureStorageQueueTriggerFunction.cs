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
        ILogger logger,
        ExecutionContext context)
    {
        await endpoint.Process(message, context, logger);
    }

    #endregion

    #region EndpointSetup

    private static FunctionEndpoint endpoint = new FunctionEndpoint(executionContext =>
    {
        var configuration = new StorageQueueTriggeredEndpointConfiguration(EndpointName, executionContext.Logger);

        configuration.UseSerialization<NewtonsoftSerializer>();

        return configuration;
    });

    #endregion

    #region AlternativeEndpointSetup

    private static readonly FunctionEndpoint autoConfiguredEndpoint = new FunctionEndpoint(executionContext =>
    {
        // endpoint name, logger, and connection strings are automatically derived from FunctionName and QueueTrigger attributes
        var configuration = StorageQueueTriggeredEndpointConfiguration.CreateUsingFunctionAndTriggerAttributesInformation(executionContext);

        configuration.UseSerialization<NewtonsoftSerializer>();

        return configuration;
    });

    #endregion

}