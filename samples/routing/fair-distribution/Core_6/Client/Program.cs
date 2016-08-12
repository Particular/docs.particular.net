using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.FairDistribution.Client";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();

        var endpointConfiguration = new EndpointConfiguration("Samples.FairDistribution.Client");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        AddRouting(endpointConfiguration);

        AddFairDistributionClient(endpointConfiguration);

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            Console.WriteLine("Press enter to send a message");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                var orderId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
                Console.WriteLine($"Placing order {orderId}");
                var message = new PlaceOrder
                {
                    OrderId = orderId,
                    Value = random.Next(100)
                };
                await endpointInstance.Send(message).ConfigureAwait(false);
            }

        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }

    static void AddFairDistributionClient(EndpointConfiguration endpointConfiguration)
    {
        #region FairDistributionClient

        endpointConfiguration.EnableFeature<FairDistribution>();
        var routing = endpointConfiguration.UseTransport<MsmqTransport>().Routing();
        var settings = endpointConfiguration.GetSettings();
        routing.SetMessageDistributionStrategy(
            endpointName: "Samples.FairDistribution.Server",
            distributionStrategy: new FairDistributionStrategy(settings));

        #endregion
    }

    static void AddRouting(EndpointConfiguration endpointConfiguration)
    {
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();

        #region SimulateMultiMachine
        transport.SimulateMultipleMachines("Client");

        #endregion

        var routing = transport.Routing();

        #region Routing

        routing.RouteToEndpoint(typeof(PlaceOrder), "Samples.FairDistribution.Server");

        #endregion
    }
}