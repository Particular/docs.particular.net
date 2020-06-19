using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.AzureFunctions;
using NServiceBus.AzureFunctions.ServiceBus;

class Usage
{
    Usage(ServiceBusTriggeredEndpointConfiguration serviceBusTriggeredEndpointConfiguration, FunctionExecutionContext executionContext)
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
        var serviceBusTriggeredEndpointConfiguration = ServiceBusTriggeredEndpointConfiguration.FromAttributes();

        return serviceBusTriggeredEndpointConfiguration;
    });

    #endregion

    #region function-definition

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
        #region alternative-endpoint-setup

        static readonly FunctionEndpoint endpoint = new FunctionEndpoint(executionContext =>
        {
            var serviceBusTriggeredEndpointConfiguration = new ServiceBusTriggeredEndpointConfiguration("ASBTriggerQueue");

            return serviceBusTriggeredEndpointConfiguration;
        });

        #endregion
    }
}