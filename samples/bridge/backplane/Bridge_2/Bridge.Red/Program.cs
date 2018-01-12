using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Bridge;
using NServiceBus.Transport;

class Program
{
    static async Task Main()
    {
        Console.Title = "Bridge.Red";

        var bridgeConfig = Bridge
            .Between<SqlServerTransport>("Samples.Bridge.Backplane.Bridge.Red", t =>
            {
                t.ConnectionString(ConnectionStrings.Red);
                t.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
            })
            .And<RabbitMQTransport>("Samples.Bridge.Backplane.Bridge.Red", t =>
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