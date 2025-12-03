using Microsoft.Extensions.Hosting;
using NServiceBus;

// for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlMultiInstanceReceiver;Integrated Security=True;Max Pool Size=100;Encrypt=false
const string receiverConnectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlMultiInstanceReceiver;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

// for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlMultiInstanceSender;Integrated Security=True;Max Pool Size=100;Encrypt=false
const string senderConnectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlMultiInstanceSender;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

SqlHelper.EnsureDatabaseExists(receiverConnectionString);
SqlHelper.EnsureDatabaseExists(senderConnectionString);

var builder = Host.CreateApplicationBuilder();
var bridgeConfiguration = new BridgeConfiguration();

#region BridgeConfiguration

var receiverTransport = new BridgeTransport(new SqlServerTransport(receiverConnectionString))
{
    Name = "Receiver",
    AutoCreateQueues = true
};

receiverTransport.HasEndpoint("Samples.SqlServer.MultiInstanceReceiver");

var senderTransport = new BridgeTransport(new SqlServerTransport(senderConnectionString))
{
    Name = "Sender",
    AutoCreateQueues = true
};

senderTransport.HasEndpoint("Samples.SqlServer.MultiInstanceSender");

bridgeConfiguration.AddTransport(receiverTransport);
bridgeConfiguration.AddTransport(senderTransport);
bridgeConfiguration.RunInReceiveOnlyTransactionMode();

#endregion

// more configuration...

builder.UseNServiceBusBridge(bridgeConfiguration);
var host = builder.Build();
await host.RunAsync();