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
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {messageId}. Invocation {context.InvocationId}");

            serverlessEndpoint = serverlessEndpoint ?? new ServerlessEndpoint(() =>
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(context.FunctionAppDirectory)
                    .AddJsonFile("local.settings.json", optional: false)
                    .Build();

                var azureServiceBusTriggerEndpoint = new AzureServiceBusTriggerEndpoint("ASBTriggerQueue");
                azureServiceBusTriggerEndpoint.UseSerialization<NewtonsoftSerializer>();
                var transport = azureServiceBusTriggerEndpoint.UseTransportForDispatch<AzureServiceBusTransport>();
                transport.ConnectionString(config.GetValue<string>("Values:ASBConnectionString"));

                return azureServiceBusTriggerEndpoint;
            });

            await serverlessEndpoint.Process(message);
        }

        static ServerlessEndpoint serverlessEndpoint;
    }
}