using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Bridge
{
    class Program
    {
        const string ReceiverConnectionString =
            @"Data Source=.\SqlExpress;Database=NsbSamplesSqlMultiInstanceReceiver;Integrated Security=True;Max Pool Size=100";

        const string SenderConnectionString =
            @"Data Source=.\SqlExpress;Database=NsbSamplesSqlMultiInstanceSender;Integrated Security=True;Max Pool Size=100";

        public static async Task Main(string[] args)
        {
            SqlHelper.EnsureDatabaseExists(ReceiverConnectionString);
            SqlHelper.EnsureDatabaseExists(SenderConnectionString);

            await Host.CreateDefaultBuilder()
                .UseNServiceBusBridge((hostBuilderContext, bridgeConfiguration) =>
                {
                    #region BridgeConfiguration

                    var receiverTransport = new BridgeTransport(new SqlServerTransport(ReceiverConnectionString))
                    {
                        Name = "Receiver",
                        AutoCreateQueues = true
                    };

                    receiverTransport.HasEndpoint("Samples.SqlServer.MultiInstanceReceiver");

                    var senderTransport = new BridgeTransport(new SqlServerTransport(SenderConnectionString))
                    {
                        Name = "Sender",
                        AutoCreateQueues = true
                    };

                    senderTransport.HasEndpoint("Samples.SqlServer.MultiInstanceSender");

                    bridgeConfiguration.AddTransport(receiverTransport);
                    bridgeConfiguration.AddTransport(senderTransport);

                    #endregion

                    // more configuration...
                })
                .Build()
                .RunAsync().ConfigureAwait(false);
        }
    }
}