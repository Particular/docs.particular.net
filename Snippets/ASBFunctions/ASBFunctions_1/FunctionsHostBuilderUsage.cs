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

        #region asb-enable-diagnostics
        class EnableDiagnosticsOnStartup : FunctionsStartup
        {
            public override void Configure(IFunctionsHostBuilder builder)
            {
                builder.UseNServiceBus(() => {
                    var configuration = new ServiceBusTriggeredEndpointConfiguration("MyFunctionsEndpoint");
                    configuration.AdvancedConfiguration.SendFailedMessagesTo("error");
                    return configuration;
               });
            }
        }
        #endregion

        #region asb-configure-error-queue
        class configureErrorQueueuOnStartup : FunctionsStartup
        {
            public override void Configure(IFunctionsHostBuilder builder)
            {
                builder.UseNServiceBus(() => {
                    var configuration = new ServiceBusTriggeredEndpointConfiguration("MyFunctionsEndpoint");
                    configuration.LogDiagnostics();
                    return configuration;
                });
            }
        }
        #endregion

        #region asb-function-hostbuilder-trigger
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
                [ServiceBusTrigger(queueName: "MyFunctionsEndpoint")]
                Message message,
                ILogger logger,
                ExecutionContext executionContext)
            {
                await endpoint.Process(message, executionContext, logger);
            }
        }
        #endregion

        #region asb-dispatching-outside-message-handler
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