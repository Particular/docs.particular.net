using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using NServiceBus.Features;

namespace AzureFunctions.Console
{
    using System;
    
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Press [1] to send a message to the Azure ServiceBus trigger queue.");
            Console.WriteLine("Press [2] to send a message to the Azure Storage trigger queue.");
            Console.WriteLine("Press [Esc] to exit.");
            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        await SendASBMessage();
                        break;
                    case ConsoleKey.D2:
                        await SendASQMessage();
                        break;
                    case ConsoleKey.Escape:
                        await (asqEndpoint?.Stop() ?? Task.CompletedTask);
                        await (asbEndpoint?.Stop() ?? Task.CompletedTask);
                        return;
                    default:
                        break;
                }
            }
        }

        private static IEndpointInstance asqEndpoint;
        private static IEndpointInstance asbEndpoint;

        static async Task SendASQMessage()
        {
            if (asqEndpoint == null)
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile("local.settings.json", optional: false)
                    .Build();

                var endpointConfiguration = new EndpointConfiguration("FunctionsASQSender");
                endpointConfiguration.SendOnly();
                endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
                endpointConfiguration.DisableFeature<MessageDrivenSubscriptions>();

                var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
                transport.ConnectionString(config.GetValue<string>("Values:ASQConnectionString"));

                asqEndpoint = await Endpoint.Start(endpointConfiguration);
            }

            await asqEndpoint.Send("ASQTriggerQueue", new TriggerMessage());
        }

        static async Task SendASBMessage()
        {
            if (asbEndpoint == null)
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile("local.settings.json", optional: false)
                    .Build();

                var endpointConfiguration = new EndpointConfiguration("FunctionsASBSender");
                endpointConfiguration.SendOnly();
                endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

                var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
                transport.ConnectionString(config.GetValue<string>("Values:ASBConnectionString"));

                asbEndpoint = await Endpoint.Start(endpointConfiguration);
            }

            await asbEndpoint.Send("ASBTriggerQueue", new TriggerMessage());
        }
    }
}
