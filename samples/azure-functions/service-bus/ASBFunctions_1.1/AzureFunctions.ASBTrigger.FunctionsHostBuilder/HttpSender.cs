using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctions.ASBTrigger.FunctionsHostBuilder
{
    using NServiceBus;

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
}
