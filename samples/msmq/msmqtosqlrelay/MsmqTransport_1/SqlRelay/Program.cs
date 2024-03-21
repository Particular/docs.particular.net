using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Transport.SQLServer;

//using NServiceBus.Persistence;

class Program
{
    static async Task Main()
    {
        Console.Title = "SqlRelay";
        #region sqlrelay-config
        var endpointConfiguration = new EndpointConfiguration("SqlRelay");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.DisableFeature<AutoSubscribe>();
        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=PersistenceForSqlTransport;Integrated Security=True;Max Pool Size=100;Encrypt=false
        transport.ConnectionString(@"Server=localhost,1433;Initial Catalog=PersistenceForSqlTransport;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false");
        transport.NativeDelayedDelivery().DisableTimeoutManagerCompatibility();
        #endregion


        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("\r\nSqlRelay is running - This endpoint will relay all events received to subscribers. Press any key to stop program\r\n");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}