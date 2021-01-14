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
            readonly IFunctionEndpoint endpoint;

            // inject the FunctionEndpoint via dependency injection:
            public MyFunction(IFunctionEndpoint endpoint)
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

        #region asq-dispatching-outside-message-handler
        public class HttpSender
        {
            readonly IFunctionEndpoint functionEndpoint;

            public HttpSender(IFunctionEndpoint functionEndpoint)
            {
                this.functionEndpoint = functionEndpoint;
            }

            [FunctionName("HttpSender")]
            public async Task<IActionResult> Run(
                [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest request, ExecutionContext executionContext, ILogger logger)
            {
                logger.LogInformation("C# HTTP trigger function received a request.");

                var sendOptions = new SendOptions();
                sendOptions.RouteToThisEndpoint();

                await functionEndpoint.Send(new TriggerMessage(), sendOptions, executionContext, logger);

                return new OkObjectResult($"{nameof(TriggerMessage)} sent.");
            }
        }
        #endregion
    }
}