using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System.Threading.Tasks;
using NServiceBus.AzureFunctions.ServiceBus;

public class AzureServiceBusTriggerFunction
{
    private const string EndpointName = "ASBTriggerQueue";

    #region Function

    [FunctionName(EndpointName)]
    public static async Task Run(
        [ServiceBusTrigger(queueName: EndpointName)]
        Message message,
        ILogger logger,
        ExecutionContext executionContext)
    {
        await endpoint.Process(message, executionContext, logger);
    }

    #endregion

    #region EndpointSetup

    private static readonly FunctionEndpoint endpoint = new FunctionEndpoint(executionContext =>
    {
        // endpoint name, logger, and connection strings are automatically derived from FunctionName and ServiceBusTrigger attributes
        var configuration = ServiceBusTriggeredEndpointConfiguration.FromAttributes();

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
}
