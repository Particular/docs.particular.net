﻿using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using NServiceBus;
using NServiceBus.Persistence;

class Usage
{
    Usage(StorageQueueTriggeredEndpointConfiguration storageQueueTriggeredEndpointConfiguration, FunctionExecutionContext executionContext)
    {
        #region enable-diagnostics

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

    public static void EnablePersistence(StorageQueueTriggeredEndpointConfiguration endpointConfiguration)
    {
        #region enable-persistence

        var persistence = endpointConfiguration.AdvancedConfiguration.UsePersistence<AzureStoragePersistence>();
        persistence.ConnectionString("<connection-string>");

        #endregion
    }

    public static void EnableDelayedRetries(StorageQueueTriggeredEndpointConfiguration endpointConfiguration, int numberOfDelayedRetries, TimeSpan timeIncreaseBetweenDelayedRetries)
    {
        #region enable-delayed-retries

        var recoverability = endpointConfiguration.AdvancedConfiguration.Recoverability();
        recoverability.Delayed(settings =>
        {
            settings.NumberOfRetries(numberOfDelayedRetries);
            settings.TimeIncrease(timeIncreaseBetweenDelayedRetries);
        });

        #endregion

        #region configure-error-queue

        endpointConfiguration.AdvancedConfiguration.SendFailedMessagesTo("error");

        #endregion
    }

    public static void DisablePublishing(StorageQueueTriggeredEndpointConfiguration endpointConfiguration)
    {
        #region disable-publishing

        endpointConfiguration.Transport.DisablePublishing();

        #endregion
    }

    class SomeEvent { }

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

internal class AzureStoragePersistence : PersistenceDefinition { }
internal static class ConfigureFakeAzureStorage
{
    public static PersistenceExtensions<AzureStoragePersistence> ConnectionString(this PersistenceExtensions<AzureStoragePersistence> config, string connectionString) => config;
}