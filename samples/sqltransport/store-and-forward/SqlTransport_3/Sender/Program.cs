using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Settings;
using NServiceBus.Transport.SQLServer;

#pragma warning disable 618

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.SqlServer.StoreAndForwardSender";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();
        var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.StoreAndForwardSender");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();

        #region DelayedRetriesConfig

        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Delayed(
            customizations: delayed =>
            {
                delayed.NumberOfRetries(100);
                delayed.TimeIncrease(TimeSpan.FromSeconds(10));
            });

        #endregion

        recoverability.Immediate(immediate => immediate.NumberOfRetries(0));

        #region SenderConfiguration

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.EnableLegacyMultiInstanceMode(ConnectionProvider.GetConnecton);

        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register(
            stepId: "Forward",
            behavior: new ForwardBehavior(),
            description: "Forwards messages to destinations.");
        pipeline.Register("Store",
            factoryMethod: builder =>
            {
                var localAddress = builder.Build<ReadOnlySettings>().LocalAddress();
                return new SendThroughLocalQueueRoutingToDispatchConnector(localAddress);
            },
            description: "Send messages through local endpoint.");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press enter to publish a message");
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
            var orderSubmitted = new OrderSubmitted
            {
                OrderId = orderId,
                Value = random.Next(100)
            };
            await endpointInstance.Publish(orderSubmitted)
                .ConfigureAwait(false);
            Console.WriteLine($"Order {orderId} placed");
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}