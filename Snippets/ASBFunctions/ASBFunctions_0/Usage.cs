using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NServiceBus;

class Usage
{
    Usage(ServiceBusTriggeredEndpointConfiguration serviceBusTriggeredEndpointConfiguration, FunctionExecutionContext executionContext)
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

    static readonly FunctionEndpoint endpoint = new FunctionEndpoint(executionContext =>
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

        static readonly FunctionEndpoint endpoint = new FunctionEndpoint(executionContext =>
        {
            var serviceBusTriggeredEndpointConfiguration = new ServiceBusTriggeredEndpointConfiguration("ASBTriggerQueue");

            return serviceBusTriggeredEndpointConfiguration;
        });

        #endregion
    }
}
