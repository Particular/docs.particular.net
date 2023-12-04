using System;
using System.Threading.Tasks;

using NServiceBus;

class Program
{
    // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlMultiInstanceReceiver;Integrated Security=True;Max Pool Size=100;Encrypt=false
    const string ConnectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlMultiInstanceReceiver;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

    static async Task Main()
    {
        Console.Title = "Samples.SqlServer.MultiInstanceReceiver";

        #region ReceiverConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.MultiInstanceReceiver");
        endpointConfiguration.UseTransport(new SqlServerTransport(ConnectionString)
        {
            TransportTransactionMode = TransportTransactionMode.ReceiveOnly
        });

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.EnableInstallers();

        #endregion

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
