using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using NServiceBus;
using NServiceBus.Persistence;

class StaticUsage
{
    StaticUsage(StorageQueueTriggeredEndpointConfiguration storageQueueTriggeredEndpointConfiguration, FunctionExecutionContext executionContext)
    {
        #region asq-enable-diagnostics

        storageQueueTriggeredEndpointConfiguration.LogDiagnostics();

        #endregion
    }

    #region asq-endpoint-configuration

    static readonly IFunctionEndpoint endpoint = new FunctionEndpoint(executionContext =>
    {
        var storageQueueTriggeredEndpointConfiguration = StorageQueueTriggeredEndpointConfiguration.FromAttributes();

        return storageQueueTriggeredEndpointConfiguration;
    });

    #endregion

    public static void EnablePersistence(StorageQueueTriggeredEndpointConfiguration endpointConfiguration)
    {
        #region asq-enable-persistence

        var persistence = endpointConfiguration.AdvancedConfiguration.UsePersistence<AzureStoragePersistence>();
        persistence.ConnectionString("<connection-string>");

        #endregion
    }

    public static void EnableDelayedRetries(StorageQueueTriggeredEndpointConfiguration endpointConfiguration, int numberOfDelayedRetries, TimeSpan timeIncreaseBetweenDelayedRetries)
    {
        #region asq-enable-delayed-retries

        var recoverability = endpointConfiguration.AdvancedConfiguration.Recoverability();
        recoverability.Delayed(settings =>
        {
            settings.NumberOfRetries(numberOfDelayedRetries);
            settings.TimeIncrease(timeIncreaseBetweenDelayedRetries);
        });

        #endregion

        #region asq-configure-error-queue

        endpointConfiguration.AdvancedConfiguration.SendFailedMessagesTo("error");

        #endregion
    }

    public static void DisablePublishing(StorageQueueTriggeredEndpointConfiguration endpointConfiguration)
    {
        #region asq-disable-publishing

        endpointConfiguration.Transport.DisablePublishing();

        #endregion
    }

    class SomeEvent { }

    #region asq-function-definition

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
        #region asq-alternative-endpoint-setup

        static readonly IFunctionEndpoint endpoint = new FunctionEndpoint(executionContext =>
        {
            var storageQueueTriggeredEndpointConfiguration = new StorageQueueTriggeredEndpointConfiguration("ASQTriggerQueue");

            return storageQueueTriggeredEndpointConfiguration;
        });

        #endregion
    }

    public static class HttpSender
    {
        #region asq-static-dispatching-outside-message-handler

        [FunctionName("HttpSender")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest request, ExecutionContext executionContext, ILogger logger)
        {
            logger.LogInformation("C# HTTP trigger function received a request.");

            var sendOptions = new SendOptions();
            sendOptions.SetDestination("DestinationEndpointName");

            await functionEndpoint.Send(new TriggerMessage(), sendOptions, executionContext, logger);

            return new OkObjectResult($"{nameof(TriggerMessage)} sent.");
        }

        #endregion
    }

    #region asq-static-trigger-endpoint

    private static readonly IFunctionEndpoint functionEndpoint = new FunctionEndpoint(executionContext =>
    {
        var configuration = new StorageQueueTriggeredEndpointConfiguration("HttpSender");

        configuration.AdvancedConfiguration.SendOnly();

        return configuration;
    });

    #endregion
}

internal class AzureStoragePersistence : PersistenceDefinition { }
internal static class ConfigureFakeAzureStorage
{
    public static PersistenceExtensions<AzureStoragePersistence> ConnectionString(this PersistenceExtensions<AzureStoragePersistence> config, string connectionString) => config;
}