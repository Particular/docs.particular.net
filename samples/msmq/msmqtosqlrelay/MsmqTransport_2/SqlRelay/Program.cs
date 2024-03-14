using System;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

//using NServiceBus.Persistence;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.MsmqToSqlRelay.SqlRelay";
        #region sqlrelay-config
        var endpointConfiguration = new EndpointConfiguration("SqlRelay");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.DisableFeature<AutoSubscribe>();

        // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=PersistenceForSqlTransport;Integrated Security=True;Max Pool Size=100;Encrypt=false
        endpointConfiguration.UseTransport(new SqlServerTransport(@"Server=localhost,1433;Initial Catalog=PersistenceForSqlTransport;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false"));
        #endregion


        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("\r\nSqlRelay is running - This endpoint will relay all events received to subscribers. Press any key to stop program\r\n");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}