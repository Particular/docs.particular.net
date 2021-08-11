using NServiceBus;

// needs to be top level first defined for C# to compile
#region endpoint-trigger-function-wireup

[assembly: NServiceBusTriggerFunction("MyFunctionsEndpoint")]

#endregion

namespace ASBFunctions_1_3
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using NServiceBus;

    public class FunctionsHostBuilderUsage
    {
        class IConfigurationUsage
        {
            #region asb-function-default
            class Startup : FunctionsStartup
            {
                public override void Configure(IFunctionsHostBuilder builder)
                {
                    builder.UseNServiceBus();
                }
            }
            #endregion
        }

        #region asb-function-hostbuilder
        class HostBuilderStartup
        {
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
        }
        #endregion

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