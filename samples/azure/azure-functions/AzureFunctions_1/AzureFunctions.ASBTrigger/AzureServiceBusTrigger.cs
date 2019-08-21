using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.AzureFunctions.AzureServiceBus;
using NServiceBus.Serverless;

namespace AzureFunctions.ASBTrigger
{
    public class AzureServiceBusTrigger
    {
        [FunctionName(nameof(AzureServiceBusTrigger))]
        public static async Task Run(
            [ServiceBusTrigger(queueName: "ASBTriggerQueue", Connection = "ASBConnectionString")]
            Message message,
            string messageId,
            ILogger log,
            ExecutionContext context)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {messageId}");

            //with caching
            serverlessEndpoint = serverlessEndpoint ?? new ServerlessEndpoint(() =>
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(context.FunctionAppDirectory)
                    .AddJsonFile("local.settings.json", optional: false)
                    .Build();

                var azureServiceBusTriggerEndpoint = new AzureServiceBusTriggerEndpoint(context.FunctionName);
                //TODO: package conflicts with json serializer with functions
                azureServiceBusTriggerEndpoint.UseSerialization<NewtonsoftSerializer>();
                var transport = azureServiceBusTriggerEndpoint.UseTransportForDispatch<AzureServiceBusTransport>();
                var connectionString = config.GetValue<string>("Values:ASBConnectionString");
                transport.ConnectionString(connectionString);
                transport.Routing().RouteToEndpoint(typeof(ASQMessage), "ASQTriggerQueue");

                return azureServiceBusTriggerEndpoint;
            });

            await serverlessEndpoint.Process(message);
        }

        static ServerlessEndpoint serverlessEndpoint;
    }
}