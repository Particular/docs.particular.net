using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main(string[] args)
    {
        Console.Title = "Sender.V6";

        var endpointConfiguration = new EndpointConfiguration("Samples.Scaleout.Sender.V6");

        var routing = endpointConfiguration.UseTransport<MsmqTransport>().Routing();
        var workerEndpoint = "Samples.Scaleout.Worker";
        routing.RouteToEndpoint(typeof(PlaceOrder), workerEndpoint);
        routing.RouteToEndpoint(typeof(PlaceInvalidOrder), workerEndpoint);

        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        var conventions = endpointConfiguration.Conventions();
        conventions.DefiningMessagesAs(
            type =>
            {
                return type.GetInterfaces().Contains(typeof(IMessage));
            });

        Run(endpointConfiguration).GetAwaiter().GetResult();
    }

    static async Task Run(EndpointConfiguration busConfiguration)
    {
        var endpointInstance = await Endpoint.Start(busConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press 'A' to send a valid message or 'B' to send invalid one.");
        Console.WriteLine("Press any other key to exit.");
        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            var orderId = Guid.NewGuid();
            if (key.Key  == ConsoleKey.A)
            {
                await endpointInstance.Send(new PlaceOrder
                {
                    OrderId = orderId
                });

                Console.WriteLine($"Sent PlacedOrder command with order id {orderId}");
            }
            else if (key.Key == ConsoleKey.B)
            {
                await endpointInstance.Send(new PlaceInvalidOrder
                {
                    OrderId = orderId
                });

                Console.WriteLine($"Sent PlacedOrder command with order id {orderId}");
            }
            else
            {
                await endpointInstance.Stop().ConfigureAwait(false);
                return;
            }
        }
    }
}
