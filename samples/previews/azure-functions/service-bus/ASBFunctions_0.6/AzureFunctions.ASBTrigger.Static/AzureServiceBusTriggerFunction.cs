using System;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

public class AzureServiceBusTriggerFunction
{
    internal const string EndpointName = "ASBTriggerQueue";

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

    private static readonly IFunctionEndpoint endpoint = new FunctionEndpoint(executionContext =>
    {
        // endpoint name, and connection strings are automatically derived from FunctionName and ServiceBusTrigger attributes
        var configuration = ServiceBusTriggeredEndpointConfiguration.FromAttributes();

        // optional: log startup diagnostics using Functions provided logger
        configuration.LogDiagnostics();

        // register custom service in the container
        configuration.AdvancedConfiguration.RegisterComponents(r => r.AddSingleton(_ =>
        {
            var customComponentInitializationValue = Environment.GetEnvironmentVariable("CustomComponentValue");
            return new CustomComponent(customComponentInitializationValue);
        }));

        return configuration;
    });

    #endregion
}
