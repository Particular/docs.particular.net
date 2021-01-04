using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using NServiceBus;
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

    private static readonly IFunctionEndpoint endpoint = new FunctionEndpoint(executionContext =>
    {
        // endpoint name, logger, and connection strings are automatically derived from FunctionName and QueueTrigger attributes
        var configuration = StorageQueueTriggeredEndpointConfiguration.FromAttributes();

        configuration.UseSerialization<NewtonsoftSerializer>();

        // optional: log startup diagnostics using Functions provided logger
        configuration.LogDiagnostics();

        // Disable persistence requirement
        configuration.Transport.DisablePublishing();

        configuration.AdvancedConfiguration.RegisterComponents(r => r.ConfigureComponent(() =>
        {
            var customComponentInitializationValue = Environment.GetEnvironmentVariable("CustomComponentValue");
            return new CustomComponent(customComponentInitializationValue);
        }, DependencyLifecycle.SingleInstance));

        return configuration;
    });

    #endregion
}