using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Routing;
using NServiceBus.Support;

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
        var unicastRouting = endpointConfiguration.UnicastRouting();
        unicastRouting.Mapping.SetMessageDistributionStrategy(
            new FairDistributionStrategy(endpointConfiguration.GetSettings()), type => true);

        #endregion
    }

    static void AddRouting(EndpointConfiguration endpointConfiguration)
    {
        #region Routing

        const string server = "Samples.FairDistribution.Server";
        var unicastRouting = endpointConfiguration.UnicastRouting();
        unicastRouting.RouteToEndpoint(typeof(PlaceOrder), server);
        unicastRouting.Mapping.Physical.Add(
            new EndpointInstance(server, "1").AtMachine(RuntimeEnvironment.MachineName),
            new EndpointInstance(server, "2").AtMachine(RuntimeEnvironment.MachineName));

        #endregion
    }
}