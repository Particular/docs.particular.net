using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Bridge;

class Program
{
    static async Task Main()
    {
        Console.Title = "Bridge.Red";

        var bridgeConfig = Bridge
            .Between<SqlServerTransport>("Red", t =>
            {
                t.ConnectionString(ConnectionStrings.Red);
                t.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
            })
            .And<RabbitMQTransport>("Red", t =>
            {
                t.ConnectionString("host=localhost");
                t.UseConventionalRoutingTopology();
            });

        bridgeConfig.AutoCreateQueues();
        bridgeConfig.UseSubscriptionPersistence<InMemoryPersistence>((config, persistence) => { });

        bridgeConfig.InterceptForwarding(FuncUtils.Fold(
            Logger.Log, 
            Duplicator.DuplicateRabbitMQMessages, 
            new Deduplicator(ConnectionStrings.Red).DeduplicateSQLMessages));

        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Red);
        SqlHelper.CreateReceivedMessagesTable(ConnectionStrings.Red);

        var bridge = bridgeConfig.Create();

        await bridge.Start().ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit");
        Console.ReadLine();

        await bridge.Stop().ConfigureAwait(false);
    }
}