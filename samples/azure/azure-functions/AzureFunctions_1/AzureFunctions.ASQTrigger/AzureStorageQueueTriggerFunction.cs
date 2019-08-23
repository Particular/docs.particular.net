using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using NServiceBus;
using NServiceBus.AzureFunctions.AzureStorageQueues;
using NServiceBus.Serverless;

namespace AzureFunctions.ASQTrigger
{
    public class AzureStorageQueueTriggerFunction
    {
        [FunctionName(nameof(AzureStorageQueueTriggerFunction))]
        public static async Task QueueTrigger(
            [QueueTrigger("ASQTriggerQueue", Connection = "ASQConnectionString")]
            CloudQueueMessage myQueueItem,
            ILogger log,
            ExecutionContext executionContext)
        {
            await serverlessEndpoint.Process(myQueueItem, executionContext);
        }

        static FunctionEndpoint serverlessEndpoint = new FunctionEndpoint(executionContext =>
        {
            var config = new ConfigurationBuilder()
                     .SetBasePath(executionContext.FunctionAppDirectory)
                     .AddJsonFile("local.settings.json", optional: false)
                     .Build();

            var azureServiceBusTriggerEndpoint = new StorageQueueTriggeredEndpointConfiguration("ASQTriggerQueue");
            azureServiceBusTriggerEndpoint.UseSerialization<NewtonsoftSerializer>();

            var transport = azureServiceBusTriggerEndpoint.UseTransportForDispatch<AzureStorageQueueTransport>();
            transport.ConnectionString(config.GetValue<string>("Values:ASQConnectionString"));

            return azureServiceBusTriggerEndpoint;
        });
    }
}