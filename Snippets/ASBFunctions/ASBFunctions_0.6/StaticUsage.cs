using System;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NServiceBus;

class StaticUsage
{
    StaticUsage(ServiceBusTriggeredEndpointConfiguration serviceBusTriggeredEndpointConfiguration, FunctionExecutionContext executionContext)
    {
        #region asb-enable-diagnostics

        serviceBusTriggeredEndpointConfiguration.LogDiagnostics();

        #endregion
    }

    public static void EnableDelayedRetries(ServiceBusTriggeredEndpointConfiguration serviceBusTriggeredEndpointConfiguration)
    {
        #region asb-configure-error-queue

        serviceBusTriggeredEndpointConfiguration.AdvancedConfiguration.SendFailedMessagesTo("error");

        #endregion
    }

    #region asb-endpoint-configuration

    static readonly IFunctionEndpoint endpoint = new FunctionEndpoint(executionContext =>
    {
        var serviceBusTriggeredEndpointConfiguration = ServiceBusTriggeredEndpointConfiguration.FromAttributes();
        // customize configuration here
        return serviceBusTriggeredEndpointConfiguration;
    });

    #endregion

    #region asb-function-definition

    [FunctionName("ASBTriggerQueue")]
    public static async Task Run(
        [ServiceBusTrigger(queueName: "ASBTriggerQueue")]
        Message message,
        ILogger logger,
        ExecutionContext executionContext)
    {
        await endpoint.Process(message, executionContext, logger);
    }

    #endregion

    class AlternativeConfiguration
    {
        #region asb-alternative-endpoint-setup

        static readonly IFunctionEndpoint endpoint = new FunctionEndpoint(executionContext =>
        {
            var serviceBusTriggeredEndpointConfiguration = new ServiceBusTriggeredEndpointConfiguration("ASBTriggerQueue");

            return serviceBusTriggeredEndpointConfiguration;
        });

        #endregion
    }

    public static class HttpSender
    {
        #region asb-static-dispatching-outside-message-handler

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

    #region asb-static-trigger-endpoint

    private static readonly IFunctionEndpoint functionEndpoint = new FunctionEndpoint(executionContext =>
    {
        var configuration = new ServiceBusTriggeredEndpointConfiguration("HttpSender");

        configuration.AdvancedConfiguration.SendOnly();

        return configuration;
    });

    #endregion
}
