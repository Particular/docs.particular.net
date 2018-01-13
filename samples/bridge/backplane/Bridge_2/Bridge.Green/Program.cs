using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Bridge;

class Program
{
    static async Task Main()
    {
        Console.Title = "Bridge.Green";

        var bridgeConfig = Bridge
            .Between<SqlServerTransport>("Green", t =>
            {
                t.ConnectionString(ConnectionStrings.Green);
                t.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
            })
            .And<RabbitMQTransport>("Green", t =>
            {
                t.ConnectionString("host=localhost");
                t.UseConventionalRoutingTopology();
            });

        bridgeConfig.AutoCreateQueues();
        bridgeConfig.UseSubscriptionPersistence<InMemoryPersistence>((config, persistence) => { });

        #region GreenForwarding

        bridgeConfig.Forwarding.RegisterPublisher("OrderAccepted", "Red");

        #endregion

        bridgeConfig.InterceptForwarding(FuncUtils.Fold(
            Logger.Log,
            Duplicator.DuplicateRabbitMQMessages,
            new Deduplicator(ConnectionStrings.Blue).DeduplicateSQLMessages));

        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Green);
        SqlHelper.CreateReceivedMessagesTable(ConnectionStrings.Green);

        var bridge = bridgeConfig.Create();

        await bridge.Start().ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit");
        Console.ReadLine();

        await bridge.Stop().ConfigureAwait(false);
    }


}