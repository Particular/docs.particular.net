using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using NServiceBus;
using NServiceBus.AzureFunctions;
using NServiceBus.AzureFunctions.StorageQueues;

class Usage
{
    Usage(StorageQueueTriggeredEndpointConfiguration storageQueueTriggeredEndpointConfiguration, FunctionExecutionContext executionContext)
    {
        #region custom-diagnostics

        storageQueueTriggeredEndpointConfiguration.LogDiagnostics();

        #endregion
    }

    #region endpoint-configuration

    static readonly FunctionEndpoint endpoint = new FunctionEndpoint(executionContext =>
    {
        var storageQueueTriggeredEndpointConfiguration = StorageQueueTriggeredEndpointConfiguration.FromAttributes();

        return storageQueueTriggeredEndpointConfiguration;
    });

    #endregion

    #region function-definition

    [FunctionName("ASQTriggerQueue")]
    public static async Task Run(
        [QueueTrigger(queueName: "ASQTriggerQueue")]
        CloudQueueMessage message,
        ILogger logger,
        ExecutionContext executionContext)
    {
        await endpoint.Process(message, executionContext, logger);
    }

    #endregion

    class AlternativeConfiguration
    {
        #region alternative-endpoint-setup

        static readonly FunctionEndpoint endpoint = new FunctionEndpoint(executionContext =>
        {
            var storageQueueTriggeredEndpointConfiguration = new StorageQueueTriggeredEndpointConfiguration("ASQTriggerQueue");

            return storageQueueTriggeredEndpointConfiguration;
        });

        #endregion
    }
}