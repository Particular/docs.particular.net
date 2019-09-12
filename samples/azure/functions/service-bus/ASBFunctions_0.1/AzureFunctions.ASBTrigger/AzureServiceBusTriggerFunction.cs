using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System.Threading.Tasks;
using NServiceBus.AzureFunctions.ServiceBus;

public class AzureServiceBusTriggerFunction
{
    private const string EndpointName = "ASBTriggerQueue";
    private const string ConnectionStringName = "ASBConnectionString";

    #region Function

    [FunctionName(EndpointName)]
    public static async Task Run(
        [ServiceBusTrigger(queueName: EndpointName, Connection = ConnectionStringName )]
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
        var configuration = new ServiceBusTriggeredEndpointConfiguration(EndpointName, ConnectionStringName);
        configuration.UseSerialization<NewtonsoftSerializer>();

        // optional: log startup diagnostics using Functions provided logger
        configuration.AdvancedConfiguration.CustomDiagnosticsWriter(diagnostics =>
        {
            executionContext.Logger.LogInformation(diagnostics);
            return Task.CompletedTask;
        });

        return configuration;
    });

    #endregion EndpointSetup

    #region AlternativeEndpointSetup

    private static readonly FunctionEndpoint autoConfiguredEndpoint = new FunctionEndpoint(executionContext =>
    {
        // endpoint name, logger, and connection strings are automatically derived from FunctionName and ServiceBusTrigger attributes
        var configuration = ServiceBusTriggeredEndpointConfiguration.FromAttributes();

        configuration.UseSerialization<NewtonsoftSerializer>();

        return configuration;
    });

    #endregion
}
