using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Bridge
{
    class Program
    {
        // const string ReceiverConnectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlMultiInstanceReceiver;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";
        // for SqlExpress and local instance
        const string ReceiverConnectionString = @"Data Source=(localdb)\mssqllocaldb;Database=NsbSamplesSqlMultiInstanceReceiver;Trusted_Connection=True;MultipleActiveResultSets=true";

        //const string SenderConnectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlMultiInstanceSender;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";
        // for SqlExpress and local instance
        const string SenderConnectionString = @"Data Source=(localdb)\mssqllocaldb;Database=NsbSamplesSqlMultiInstanceSender;Trusted_Connection=True;MultipleActiveResultSets=true";

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
                .RunAsync();
        }
    }
}