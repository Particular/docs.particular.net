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
            .Between<SqlServerTransport>("Samples.Bridge.Backplane.Bridge.Blue", t =>
            {
                t.ConnectionString(ConnectionStrings.Blue);
                t.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
            })
            .And<RabbitMQTransport>("Samples.Bridge.Backplane.Bridge.Blue", t =>
            {
                t.ConnectionString("host=localhost");
                t.UseConventionalRoutingTopology();
            });

        bridgeConfig.AutoCreateQueues();
        bridgeConfig.UseSubscriptionPersistence<InMemoryPersistence>((config, persistence) => { });

        #endregion

        #region BridgeForwarding

        bridgeConfig.Forwarding.ForwardTo("MyMessage", "Samples.Bridge.Backplane.Bridge.Red");
        bridgeConfig.Forwarding.RegisterPublisher("MyEvent", "Samples.Bridge.Backplane.Bridge.Red");

        #endregion

        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Blue);
        SqlHelper.CreateReceivedMessagesTable(ConnectionStrings.Blue);

        #region BridgeInterception

        bridgeConfig.InterceptForwarding(FuncUtils.Fold(
            Logger.Log,
            Duplicator.DuplicateRabbitMQMessages,
            new Deduplicator(ConnectionStrings.Blue).DeduplicateSQLMessages));
        
        #endregion

        var bridge = bridgeConfig.Create();

        await bridge.Start().ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit");
        Console.ReadLine();

        await bridge.Stop().ConfigureAwait(false);
    }

    
}