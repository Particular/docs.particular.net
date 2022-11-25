using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using NServiceBus;

#region asb-function-default
[assembly: FunctionsStartup(typeof(Startup))]
[assembly: NServiceBusTriggerFunction(Startup.EndpointName)]

class Startup : FunctionsStartup
{
    public const string EndpointName = "MyFunctionsEndpoint";

    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.UseNServiceBus(() => new ServiceBusTriggeredEndpointConfiguration(EndpointName));
    }
}
#endregion

namespace ASBFunctions_2_0
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using NServiceBus;

    #region asb-function-hostbuilder
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.UseNServiceBus(() =>
            {
                var configuration = new ServiceBusTriggeredEndpointConfiguration("MyFunctionsEndpoint");
                configuration.Transport.ConnectionString("functionConnectionString");
                return configuration;
            });
        }
    }
    #endregion

    public class FunctionsHostBuilderUsage
    {
        #region asb-configure-error-queue
        class EnableDiagnosticsOnStartup : FunctionsStartup
        {
            public override void Configure(IFunctionsHostBuilder builder)
            {
                builder.UseNServiceBus(() =>
                {
                    var configuration = new ServiceBusTriggeredEndpointConfiguration("MyFunctionsEndpoint");
                    configuration.AdvancedConfiguration.SendFailedMessagesTo("error");
                    return configuration;
                });
            }
        }
        #endregion

        class ConfigureErrorQueueOnStartup : FunctionsStartup
        {
            #region asb-enable-diagnostics
            public override void Configure(IFunctionsHostBuilder builder)
            {
                builder.UseNServiceBus(() =>
                {
                    var configuration = new ServiceBusTriggeredEndpointConfiguration("MyFunctionsEndpoint");
                    configuration.LogDiagnostics();
                    return configuration;
                });
            }
            #endregion
        }

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