using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Bridge;
using NServiceBus.Transport;

class Program
{
    static async Task Main()
    {
        Console.Title = "Bridge.Red";

        #region BridgeConfig

        var bridgeConfig = Bridge
            .Between<SqlServerTransport>("Samples.Bridge.Backplane.Bridge.Red", t =>
            {
                t.ConnectionString(ConnectionStrings.Red);
                t.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
            })
            .And<RabbitMQTransport>("Samples.Bridge.Backplane.Bridge.Red.Rabbit", t =>
            {
                t.ConnectionString("host=localhost");
                t.UseConventionalRoutingTopology();
            });

        bridgeConfig.AutoCreateQueues();
        bridgeConfig.UseSubscriptionPersistence<InMemoryPersistence>((config, persistence) => { });

        #endregion

        bridgeConfig.InterceptForwarding((queue, message, dispatch, forward) =>
        {
            return forward(async (messages, transaction, context) =>
            {
                transaction.TryGet<SqlConnection>(out var conn);
                if (conn != null)
                {
                    //Duplicate messages coming from SQL Server to RabbitMQ to simulate connectivity problems.
                    await dispatch(messages, transaction, context);
                    await dispatch(messages, transaction, context);
                }
                else
                {
                    await dispatch(messages, transaction, context);
                }
            });
        });

        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Red);
        SqlHelper.CreateReceivedMessagesTable(ConnectionStrings.Red);

        var bridge = bridgeConfig.Create();

        await bridge.Start().ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit");
        Console.ReadLine();

        await bridge.Stop().ConfigureAwait(false);
    }
}