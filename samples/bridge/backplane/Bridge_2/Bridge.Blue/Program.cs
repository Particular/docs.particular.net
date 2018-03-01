using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Bridge;

class Program
{
    static async Task Main()
    {
        Console.Title = "Bridge.Blue";

        #region BridgeConfig

        var bridgeConfig = Bridge
            .Between<SqlServerTransport>("Blue-SQL", t =>
            {
                t.ConnectionString(ConnectionStrings.Blue);
                t.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
            })
            .And<RabbitMQTransport>("Blue-Rabbit", t =>
            {
                t.ConnectionString("host=localhost");
                t.UseConventionalRoutingTopology();
            });

        bridgeConfig.AutoCreateQueues();
        bridgeConfig.UseSubscriptionPersistence(new InMemorySubscriptionStorage());

        #endregion

        #region BlueForwarding

        bridgeConfig.Forwarding.ForwardTo("PlaceOrder", "Red-Rabbit");

        #endregion

        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Blue);
        SqlHelper.CreateReceivedMessagesTable(ConnectionStrings.Blue);

        bridgeConfig.InterceptForwarding(FuncUtils.Fold(
            Logger.Log,
            Duplicator.DuplicateRabbitMQMessages,
            new Deduplicator(ConnectionStrings.Blue).DeduplicateSQLMessages));

        var bridge = bridgeConfig.Create();

        await bridge.Start().ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit");
        Console.ReadLine();

        await bridge.Stop().ConfigureAwait(false);
    }

    
}