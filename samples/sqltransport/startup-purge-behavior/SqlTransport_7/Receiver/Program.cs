using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SqlServer.TruncateReceiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.TruncateReceiver");
        // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=SQLServerTruncate;Integrated Security=True;Encrypt=false
        var connectionString = @"Server=localhost,1433;Initial Catalog=SQLServerTruncate;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";

        endpointConfiguration.UseTransport(new SqlServerTransport(connectionString)
        {
            TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive
        });

        endpointConfiguration.EnableInstallers();

        await SqlHelper.EnsureDatabaseExists(connectionString);
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.WriteLine("Waiting for message from the Sender");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}