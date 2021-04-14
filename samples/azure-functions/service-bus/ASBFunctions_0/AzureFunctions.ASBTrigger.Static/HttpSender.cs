using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctions.ASBTrigger.Static
{
    using NServiceBus;

    public static class HttpSender
    {
        private const string EndpointName = "HttpSender";

        [FunctionName(EndpointName)]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest request, ExecutionContext executionContext, ILogger logger)
        {
            logger.LogInformation("C# HTTP trigger function received a request.");

            var sendOptions = new SendOptions();
            sendOptions.SetDestination(AzureServiceBusTriggerFunction.EndpointName);

            await endpoint.Send(new TriggerMessage(), sendOptions, executionContext, logger);

            return new OkObjectResult($"{nameof(TriggerMessage)} sent.");
        }

        private static readonly IFunctionEndpoint endpoint = new FunctionEndpoint(executionContext =>
        {
            var configuration = new ServiceBusTriggeredEndpointConfiguration(EndpointName);

            configuration.AdvancedConfiguration.SendOnly();

            return configuration;
        });
    }
}
