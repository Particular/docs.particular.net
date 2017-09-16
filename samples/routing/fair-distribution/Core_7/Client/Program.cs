using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Configuration.AdvancedExtensibility;

class Program
{

    static async Task Main()
    {
        Console.Title = "Samples.FairDistribution.Client";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();

        var endpointConfiguration = new EndpointConfiguration("Samples.FairDistribution.Client");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        AddRouting(endpointConfiguration);

        AddFairDistributionClient(endpointConfiguration);

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

    static void AddFairDistributionClient(EndpointConfiguration endpointConfiguration)
    {
        #region FairDistributionClient

        endpointConfiguration.EnableFeature<FairDistribution>();
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        var routing = transport.Routing();
        var settings = endpointConfiguration.GetSettings();
        var strategy = new FairDistributionStrategy(
            settings: settings,
            endpoint: "Samples.FairDistribution.Server",
            scope: DistributionStrategyScope.Send);
        routing.SetMessageDistributionStrategy(strategy);

        #endregion
    }

    static void AddRouting(EndpointConfiguration endpointConfiguration)
    {
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();

        var routing = transport.Routing();

        #region Routing

        routing.RouteToEndpoint(typeof(PlaceOrder), "Samples.FairDistribution.Server");

        #endregion
    }
}