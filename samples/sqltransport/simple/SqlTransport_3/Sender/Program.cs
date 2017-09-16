using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SqlServer.SimpleSender";
        var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.SimpleSender");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();

        #region TransportConfiguration

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        var connection = @"Data Source=.\SqlExpress;Database=SqlServerSimple;Integrated Security=True;Max Pool Size=100";
        transport.ConnectionString(connection);
        #endregion

        SqlHelper.EnsureDatabaseExists(connection);
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        await endpointInstance.Send("Samples.SqlServer.SimpleReceiver", new MyMessage())
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

}