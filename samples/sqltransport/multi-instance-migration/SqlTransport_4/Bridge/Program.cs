using System;

namespace Bridge
{
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Router;

    class Program
    {
        const string SenderConnectionString = @"Data Source=.\SqlExpress;Database=NsbSamplesSqlMultiInstanceSender4;Integrated Security=True;Max Pool Size=100";
        const string ReceiverConnectionString = @"Data Source=.\SqlExpress;Database=NsbSamplesSqlMultiInstanceReceiver4;Integrated Security=True;Max Pool Size=100";
        const string BridgeConnectionString = @"Data Source=.\SqlExpress;Database=NsbSamplesSqlMultiInstanceBridge4;Integrated Security=True;Max Pool Size=100";

        static async Task Main()
        {
            Console.Title = "Samples.SqlServer.MultiInstanceBridge";

            SqlHelper.EnsureDatabaseExists(SenderConnectionString);
            SqlHelper.EnsureDatabaseExists(ReceiverConnectionString);
            SqlHelper.EnsureDatabaseExists(BridgeConnectionString);

            #region BridgeConfiguration

            var storage = new SqlSubscriptionStorage(
                connectionBuilder: () => new SqlConnection(BridgeConnectionString),
                tablePrefix: "",
                sqlDialect: new SqlDialect.MsSqlServer(), 
                cacheFor: null);

            //Ensures all required schema objects are created
            await storage.Install().ConfigureAwait(false);

            var bridgeConfig = new RouterConfiguration("Bridge");
            var senderInterface = bridgeConfig.AddInterface<SqlServerTransport>("Bridge-Sender",
                t => t.ConnectionString(SenderConnectionString));
            var receiverInterface = bridgeConfig.AddInterface<SqlServerTransport>("Bridge-Receiver",
                t => t.ConnectionString(ReceiverConnectionString));

            senderInterface.UseSubscriptionPersistence(storage);
            receiverInterface.UseSubscriptionPersistence(storage);

            bridgeConfig.AutoCreateQueues();
            var routeTable = bridgeConfig.UseStaticRoutingProtocol();
            routeTable.AddForwardRoute("Bridge-Sender", "Bridge-Receiver");
            routeTable.AddForwardRoute("Bridge-Receiver", "Bridge-Sender");

            #endregion

            var bridge = Router.Create(bridgeConfig);

            await bridge.Start().ConfigureAwait(false);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            await bridge.Stop().ConfigureAwait(false);
        }
    }
}