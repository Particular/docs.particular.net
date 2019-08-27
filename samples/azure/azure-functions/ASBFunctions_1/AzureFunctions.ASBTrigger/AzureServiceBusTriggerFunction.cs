using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System.Threading.Tasks;

namespace AzureFunctions.ASBTrigger
{
    using NServiceBus.AzureFunctions.ServiceBus;

    public class AzureServiceBusTriggerFunction
    {
        private const string EndpointName = "ASBTriggerQueue";
        private const string ConnectionStringName = "ASBConnectionString";

        [FunctionName(EndpointName)]
        public static async Task Run(
            [ServiceBusTrigger(queueName: EndpointName, Connection = ConnectionStringName )]
            Message message,
            ILogger log,
            ExecutionContext executionContext)
        {
            await endpoint.Process(message, executionContext);
        }

        private static readonly FunctionEndpoint endpoint = new FunctionEndpoint(executionContext =>
        {
            var configuration = new ServiceBusTriggeredEndpointConfiguration(EndpointName, ConnectionStringName, executionContext);
            configuration.UseSerialization<NewtonsoftSerializer>();

            return configuration;
        });
    }
}
