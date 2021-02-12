using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

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
        var sqlTransport =
            new SqlServerTransport(
                @"Data Source=.\SqlExpress;Database=PersistenceForSqlTransport;Integrated Security=True");
        endpointConfiguration.UseTransport(sqlTransport);

        #endregion


        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("\r\nSqlRelay is running - This endpoint will relay all events received to subscribers. Press any key to stop program\r\n");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}