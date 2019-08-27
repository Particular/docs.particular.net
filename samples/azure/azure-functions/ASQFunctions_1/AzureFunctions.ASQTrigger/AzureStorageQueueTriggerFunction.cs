using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using NServiceBus;
using NServiceBus.AzureFunctions.StorageQueues;
using System.Threading.Tasks;

namespace AzureFunctions.ASQTrigger
{
public class AzureStorageQueueTriggerFunction
    {
        private const string EndpointName = "ASQTriggerQueue";
        private const string ConnectionStringName = "ASQConnectionString";

        [FunctionName(EndpointName)]
        public static async Task QueueTrigger(
            [QueueTrigger(EndpointName, Connection = ConnectionStringName)]
            CloudQueueMessage message,
            ILogger log,
            ExecutionContext context)
        {
            await endpoint.Process(message, context);
        }

        private static FunctionEndpoint endpoint = new FunctionEndpoint(executionContext =>
        {
            var configuration = new StorageQueueTriggeredEndpointConfiguration(EndpointName, ConnectionStringName, executionContext);

            configuration.UseSerialization<NewtonsoftSerializer>();

            return configuration;
        });
    }
}