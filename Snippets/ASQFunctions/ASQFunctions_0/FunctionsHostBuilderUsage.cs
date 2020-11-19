namespace ASQFunctions_0
{
    using System.Threading.Tasks;
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using Microsoft.WindowsAzure.Storage.Queue;
    using NServiceBus;

    public class FunctionsHostBuilderUsage
    {
        #region asq-function-hostbuilder
        class Startup : FunctionsStartup
        {
            public override void Configure(IFunctionsHostBuilder builder)
            {
                builder.UseNServiceBus(() => new StorageQueueTriggeredEndpointConfiguration("MyFunctionsEndpoint"));
            }
        }
        #endregion

        #region asq-function-hostbuilder-trigger
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
                [QueueTrigger(queueName: "MyFunctionsEndpoint")]
                CloudQueueMessage message,
                ILogger logger,
                ExecutionContext executionContext)
            {
                await endpoint.Process(message, executionContext, logger);
            }
        }
        #endregion
    }
}