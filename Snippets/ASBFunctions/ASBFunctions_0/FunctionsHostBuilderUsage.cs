namespace ASBFunctions_0
{
    using System.Threading.Tasks;
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using NServiceBus;

    public class FunctionsHostBuilderUsage
    {
        #region asb-function-hostbuilder
        class Startup : FunctionsStartup
        {
            public override void Configure(IFunctionsHostBuilder builder)
            {
                builder.UseNServiceBus(() => new ServiceBusTriggeredEndpointConfiguration("MyFunctionsEndpoint"));
            }
        }
        #endregion

        #region asb-function-hostbuilder-trigger
        class MyFunction
        {
            readonly FunctionEndpoint endpoint;

            // inject the FunctionEndpoint via dependency injection:
            public MyFunction(FunctionEndpoint endpoint)
            {
                this.endpoint = endpoint;
            }

            [FunctionName("MyFunctionsEndpoint")]
            public async Task Run(
                [ServiceBusTrigger(queueName: "MyFunctionsEndpoint")]
                Message message,
                ILogger logger,
                ExecutionContext executionContext)
            {
                await endpoint.Process(message, executionContext, logger);
            }
        }
        #endregion
    }
}