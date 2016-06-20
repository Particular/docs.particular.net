using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Logging;
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
        Console.Title = "Samples.FlowControl.Client";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();

        var endpointConfiguration = new EndpointConfiguration("Samples.FlowControl.Client");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region Routing
        const string server = "Samples.FlowControl.Server";
        endpointConfiguration.UnicastRouting().RouteToEndpoint(typeof(PlaceOrder), server);
        endpointConfiguration.UnicastRouting().Mapping.Physical.Add(
            new EndpointInstance(server, "1").AtMachine(RuntimeEnvironment.MachineName),
            new EndpointInstance(server, "2").AtMachine(RuntimeEnvironment.MachineName));
        #endregion

        #region FairDistributionClient
        endpointConfiguration.EnableFeature<FlowControl>();
        endpointConfiguration.UnicastRouting().Mapping.SetMessageDistributionStrategy(
            new ToLeastBusyDistributionStrategy(endpointConfiguration.GetSettings()), type => true);
        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            Console.WriteLine("Press enter to send a message");
            Console.WriteLine("Press any key to exit");

            #region ClientLoop

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

            #endregion
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}