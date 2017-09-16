using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;

class Program
{

    static async Task Main()
    {
        Console.Title = "Samples.InstanceMappingFile.Client";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();

        var endpointConfiguration = new EndpointConfiguration("Samples.InstanceMappingFile.Client");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region FileInstanceMapping
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        var routing = transport.Routing();
        var routingTable = routing.InstanceMappingFile();
        routingTable.FilePath(@"..\..\..\..\instance-mapping.xml");
        #endregion

        routing.RouteToEndpoint(typeof(PlaceOrder), "Samples.InstanceMappingFile.Sales");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press enter to send a message");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }
            var orderId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
            Console.WriteLine($"Placing order {orderId}");
            var message = new PlaceOrder
            {
                OrderId = orderId,
                Value = random.Next(100)
            };
            await endpointInstance.Send(message)
                .ConfigureAwait(false);
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}