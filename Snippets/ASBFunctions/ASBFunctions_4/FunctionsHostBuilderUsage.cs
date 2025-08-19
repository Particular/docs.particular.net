using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using NServiceBus;

#region asb-function-default
[assembly: FunctionsStartup(typeof(Startup))]
[assembly: NServiceBusTriggerFunction("MyFunctionsEndpoint")]

class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.UseNServiceBus();
    }
}
#endregion

namespace ASBFunctions_4_0
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using NServiceBus;
    using System.Threading.Tasks;

    #region asb-function-hostbuilder
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.UseNServiceBus(configuration =>
            {
                var transport = configuration.Transport;
                // Configure transport
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
                builder.UseNServiceBus(configuration =>
                {
                    configuration.AdvancedConfiguration.SendFailedMessagesTo("error");
                });
            }
        }
        #endregion

        class ConfigureErrorQueueOnStartup : FunctionsStartup
        {
            #region asb-enable-diagnostics
            public override void Configure(IFunctionsHostBuilder builder)
            {
                builder.UseNServiceBus(configuration =>
                {
                    configuration.LogDiagnostics();
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