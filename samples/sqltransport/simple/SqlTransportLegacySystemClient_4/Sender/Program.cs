using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport.SQLServer;

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
        // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=SqlServerSimple;Integrated Security=True;Max Pool Size=100;Encrypt=false
        var connectionString = @"Server=localhost,1433;Initial Catalog=SqlServerSimple;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";
        transport.ConnectionString(connectionString);
        #endregion

        transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
        transport.UseNativeDelayedDelivery().DisableTimeoutManagerCompatibility();

        await SqlHelper.EnsureDatabaseExists(connectionString);
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