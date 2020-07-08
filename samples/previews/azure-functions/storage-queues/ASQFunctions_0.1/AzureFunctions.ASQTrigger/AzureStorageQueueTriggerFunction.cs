﻿using Microsoft.Azure.WebJobs;
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
        var configuration = new StorageQueueTriggeredEndpointConfiguration(EndpointName);

        configuration.UseSerialization<NewtonsoftSerializer>();

        // optional: log startup diagnostics using Functions provided logger
        configuration.AdvancedConfiguration.CustomDiagnosticsWriter(diagnostics =>
        {
            executionContext.Logger.LogInformation(diagnostics);
            return Task.CompletedTask;
        });

        return configuration;
    });

    #endregion

    #region AlternativeEndpointSetup

    private static readonly FunctionEndpoint autoConfiguredEndpoint = new FunctionEndpoint(executionContext =>
    {
        // endpoint name, logger, and connection strings are automatically derived from FunctionName and QueueTrigger attributes
        var configuration = StorageQueueTriggeredEndpointConfiguration.FromAttributes();

        configuration.UseSerialization<NewtonsoftSerializer>();

        return configuration;
    });

    #endregion

}