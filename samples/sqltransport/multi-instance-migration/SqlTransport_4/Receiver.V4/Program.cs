using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
#pragma warning disable 618

class Program
{
    const string ConnectionString = @"Data Source=.\SqlExpress;Database=NsbSamplesSqlMultiInstanceReceiver4;Integrated Security=True;Max Pool Size=100";

    static async Task Main()
    {
        Console.Title = "Samples.SqlServer.MultiInstanceReceiver.V4";

        #region ReceiverConfigurationV4

        var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.MultiInstanceReceiver");
        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(ConnectionString);
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        #endregion

        endpointConfiguration.Conventions().DefiningMessagesAs(t => t.Assembly == typeof(ClientOrder).Assembly && t.Namespace == "Messages");

        SqlHelper.EnsureDatabaseExists(ConnectionString);

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.WriteLine("Waiting for Order messages from the Sender");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

}