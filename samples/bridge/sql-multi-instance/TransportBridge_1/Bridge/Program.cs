using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Bridge
{
    class Program
    {
        // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlMultiInstanceReceiver;Integrated Security=True;Max Pool Size=100;Encrypt=false
        const string ReceiverConnectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlMultiInstanceReceiver;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

        // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlMultiInstanceSender;Integrated Security=True;Max Pool Size=100;Encrypt=false
        const string SenderConnectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlMultiInstanceSender;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

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

                    // .NET 6 does not support distributed transactions
                    bridgeConfiguration.RunInReceiveOnlyTransactionMode();

                    #endregion

                    // more configuration...
                })
                .Build()
                .RunAsync().ConfigureAwait(false);
        }
    }
}