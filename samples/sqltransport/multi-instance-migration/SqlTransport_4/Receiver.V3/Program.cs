using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Transport.SQLServer;
#pragma warning disable 618

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SqlServer.MultiInstanceReceiver.V3";

        #region ReceiverConfigurationV3

        var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.MultiInstanceReceiver");
        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.EnableLegacyMultiInstanceMode(ConnectionProvider.GetConnection);
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        #endregion

        SqlHelper.EnsureDatabaseExists(ConnectionProvider.DefaultConnectionString);

        endpointConfiguration.Conventions().DefiningMessagesAs(t => t.Assembly == typeof(ClientOrder).Assembly && t.Namespace == "Messages");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.WriteLine("Waiting for Order messages from the Sender");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}