using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Logging;
using NServiceBus.Routing;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.FlowControl.Client";
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Info);

        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();

        var endpointConfiguration = new EndpointConfiguration("Samples.FlowControl.Client");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        endpointConfiguration.UnicastRouting().RouteToEndpoint(typeof(PlaceOrder), "Samples.FlowControl.Server");
        var server = new EndpointName("Samples.FlowControl.Server");
        endpointConfiguration.UnicastRouting().Mapping.Physical.Add(server,
            new EndpointInstance(server, "1").AtMachine("SIMON-MAC"),
            new EndpointInstance(server, "2").AtMachine("SIMON-MAC"));


        endpointConfiguration.EnableFeature<FlowControl>();
        endpointConfiguration.UnicastRouting().Mapping.SetMessageDistributionStrategy(
            new ControlledFlowDistributionStrategy(endpointConfiguration.GetSettings()), type => true);

        var i1 = new EndpointInstance(new EndpointName("Samples.FlowControl.Server"), "1").AtMachine("SIMON-MAC");
        var i2 = new EndpointInstance(new EndpointName("Samples.FlowControl.Server"), "1").AtMachine("SIMON-MAC");

        Console.WriteLine(i1.GetHashCode());
        Console.WriteLine(i2.GetHashCode());

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