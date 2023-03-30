using System;
using System.Threading.Tasks;
using NServiceBus;
#pragma warning disable 618

class Program
{    
    // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlMultiInstanceReceiver;Integrated Security=True;Encrypt=false
    const string ConnectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlMultiInstanceReceiver;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";

    static async Task Main()
    {
        Console.Title = "Samples.SqlServer.MultiInstanceReceiver";

        #region ReceiverConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.MultiInstanceReceiver");
        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(ConnectionString);
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