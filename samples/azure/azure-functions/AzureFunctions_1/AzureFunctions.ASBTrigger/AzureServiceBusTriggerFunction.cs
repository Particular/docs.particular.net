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
    public class AzureServiceBusTriggerFunction
    {
        [FunctionName(nameof(AzureServiceBusTriggerFunction))]
        public static async Task Run(
            [ServiceBusTrigger(queueName: "ASBTriggerQueue", Connection = "ASBConnectionString")]
            Message message,
            string messageId,
            ILogger log,
            ExecutionContext executionContext)
        {
            await serverlessEndpoint.Process(message, executionContext);
        }

        static FunctionEndpoint serverlessEndpoint = new FunctionEndpoint(executionContext =>
        {
            var config = new ConfigurationBuilder()
                   .SetBasePath(executionContext.FunctionAppDirectory)
                   .AddJsonFile("local.settings.json", optional: false)
                   .Build();

            var azureServiceBusTriggerEndpoint = new ServiceBusTriggeredEndpointConfiguration("ASBTriggerQueue");
            azureServiceBusTriggerEndpoint.UseSerialization<NewtonsoftSerializer>();
            var transport = azureServiceBusTriggerEndpoint.UseTransportForDispatch<AzureServiceBusTransport>();
            transport.ConnectionString(config.GetValue<string>("Values:ASBConnectionString"));

            return azureServiceBusTriggerEndpoint;
        });
    }
}