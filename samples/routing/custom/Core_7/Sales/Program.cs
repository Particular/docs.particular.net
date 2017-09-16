using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.CustomRouting.Sales.1";
        var endpointConfiguration = new EndpointConfiguration("Samples.CustomRouting.Sales");
        endpointConfiguration.OverrideLocalAddress("Samples.CustomRouting.Sales-1");
        endpointConfiguration.UseTransport<MsmqTransport>();
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(AutomaticRoutingConst.ConnectionString);
            });
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region EnableAutomaticRouting

        var automaticRouting = endpointConfiguration.EnableAutomaticRouting(AutomaticRoutingConst.ConnectionString);
        automaticRouting.AdvertisePublishing(typeof(OrderAccepted));

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}