using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using NServiceBus;
using NServiceBus.AzureFunctions;
using NServiceBus.AzureFunctions.StorageQueues;

class Usage
{
    Usage(StorageQueueTriggeredEndpointConfiguration serviceBusTriggeredEndpointConfiguration, FunctionExecutionContext executionContext)
    {
        #region custom-diagnostics

        serviceBusTriggeredEndpointConfiguration.AdvancedConfiguration.CustomDiagnosticsWriter(diagnostics =>
        {
            executionContext.Logger.LogInformation(diagnostics);
            return Task.CompletedTask;
        });

        #endregion
    }

    #region endpoint-configuration

    static readonly FunctionEndpoint endpoint = new FunctionEndpoint(executionContext =>
    {
        var serviceBusTriggeredEndpointConfiguration = StorageQueueTriggeredEndpointConfiguration.FromAttributes();

        return serviceBusTriggeredEndpointConfiguration;
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
            var serviceBusTriggeredEndpointConfiguration = new StorageQueueTriggeredEndpointConfiguration("ASQTriggerQueue");

            return serviceBusTriggeredEndpointConfiguration;
        });

        #endregion
    }
}