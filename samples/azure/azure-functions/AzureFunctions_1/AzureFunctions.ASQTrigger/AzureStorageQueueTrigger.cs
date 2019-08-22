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
    public class AzureStorageQueueTrigger
    {
        [FunctionName(nameof(AzureStorageQueueTrigger))]
        public static async Task QueueTrigger(
            [QueueTrigger("ASQTriggerQueue", Connection = "ASQConnectionString")]
            CloudQueueMessage myQueueItem,
            ILogger log,
            ExecutionContext context)
        {
            serverlessEndpoint = serverlessEndpoint ?? new ServerlessEndpoint(() =>
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(context.FunctionAppDirectory)
                    .AddJsonFile("local.settings.json", optional: false)
                    .Build();

                var azureServiceBusTriggerEndpoint = new AzureStorageQueueTriggerEndpoint("ASQTriggerQueue");
                azureServiceBusTriggerEndpoint.UseSerialization<NewtonsoftSerializer>();

                var transport = azureServiceBusTriggerEndpoint.UseTransportForDispatch<AzureStorageQueueTransport>();
                transport.ConnectionString(config.GetValue<string>("Values:ASQConnectionString"));
                
                return azureServiceBusTriggerEndpoint;
            });

            await serverlessEndpoint.Process(myQueueItem);
        }

        private static ServerlessEndpoint serverlessEndpoint;
    }
}