using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Blue.Client";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();

        var endpointConfiguration = new EndpointConfiguration("Blue.Client");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(ConnectionStrings.Blue);

        #region ClientRouterConfig

        var routing = transport.Routing();
        var router = routing.ConnectToRouter("Switch");
        router.RouteToEndpoint(typeof(PlaceOrder), "Red.Sales");

        #endregion

        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Blue);

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