using Messages.Events;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Shipping
{
    class Program
    {
        static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = "Shipping";

            var endpointConfig = new EndpointConfiguration("Shipping");

            var routing = endpointConfig.UseTransport<MsmqTransport>()
                .Routing();

            routing.RegisterPublisher(typeof(OrderPlaced), "Sales");
            routing.RegisterPublisher(typeof(OrderBilled), "Billing");

            endpointConfig.UseSerialization<JsonSerializer>();
            endpointConfig.UsePersistence<InMemoryPersistence>();
            endpointConfig.SendFailedMessagesTo("error");
            endpointConfig.EnableInstallers();

            var endpointInstance = await Endpoint.Start(endpointConfig)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}