using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SqlServer.SimpleReceiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.SimpleReceiver");

        var connection = @"Data Source=.\SqlExpress;Database=SqlServerSimple;Integrated Security=True;Max Pool Size=100";

        endpointConfiguration.UseTransport(new SqlServerTransport(connection)
        {
            TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive
        });

        endpointConfiguration.EnableInstallers();

        SqlHelper.EnsureDatabaseExists(connection);
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.WriteLine("Waiting for message from the Sender");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}