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
            .Between<SqlServerTransport>("Green-SQL", t =>
            {
                t.ConnectionString(ConnectionStrings.Green);
                t.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
            })
            .And<RabbitMQTransport>("Green-Rabbit", t =>
            {
                t.ConnectionString("host=localhost");
                t.UseConventionalRoutingTopology();
            });

        bridgeConfig.AutoCreateQueues();
        bridgeConfig.UseSubscriptionPersistence(new InMemorySubscriptionStorage());

        #region GreenForwarding

        bridgeConfig.Forwarding.RegisterPublisher("OrderAccepted", "Red-Rabbit");

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