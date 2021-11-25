using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureFunctions.ASBTrigger.FunctionsHostBuilder
{
    using NServiceBus;

    class ContextPublisher : IContextPublisher
    {
        private readonly IFunctionEndpoint endpoint;

        public ContextPublisher(IFunctionEndpoint endpoint)
        {
            this.endpoint = endpoint;
        }

        public async Task Do(ExecutionContext executionContext, ILogger logger)
        {
            logger.LogInformation("Send trigger message inside context publisher.");

            var sendOptions = new SendOptions();
            sendOptions.RouteToThisEndpoint();

            await endpoint.Send(new TriggerMessage(), sendOptions, executionContext);
        }
    }
}
