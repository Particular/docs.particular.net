using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Sender.V6";

        var endpointConfiguration = new EndpointConfiguration("Samples.Scaleout.Sender.V6");

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        var routing = transport.Routing();
        var workerEndpoint = "Samples.Scaleout.Worker";
        routing.RouteToEndpoint(typeof(PlaceOrder), workerEndpoint);
        routing.RouteToEndpoint(typeof(PlaceInvalidOrder), workerEndpoint);

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        var conventions = endpointConfiguration.Conventions();
        conventions.DefiningMessagesAs(
            definesMessageType: type =>
            {
                return type.GetInterfaces().Contains(typeof(IMessage));
            });

        Run(endpointConfiguration).GetAwaiter().GetResult();
    }

    static async Task Run(EndpointConfiguration endpointConfiguration)
    {
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press 'A' to send a valid message or 'B' to send an invalid one.");
        Console.WriteLine("Press any other key to exit.");
        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            var orderId = Guid.NewGuid();
            if (key.Key == ConsoleKey.A)
            {
                var placeOrder = new PlaceOrder
                {
                    OrderId = orderId
                };
                await endpointInstance.Send(placeOrder)
                    .ConfigureAwait(false);

                Console.WriteLine($"Sent PlacedOrder command with order id {orderId}");
            }
            else if (key.Key == ConsoleKey.B)
            {
                var placeInvalidOrder = new PlaceInvalidOrder
                {
                    OrderId = orderId
                };
                await endpointInstance.Send(placeInvalidOrder)
                    .ConfigureAwait(false);

                Console.WriteLine($"Sent PlacedOrder command with order id {orderId}");
            }
            else
            {
                await endpointInstance.Stop()
                    .ConfigureAwait(false);
                return;
            }
        }
    }
}