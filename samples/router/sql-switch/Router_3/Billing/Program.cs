using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Green.Billing";
        var endpointConfiguration = new EndpointConfiguration("Green.Billing");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(ConnectionStrings.Green);

        #region BillingRouterConfig

        var routing = transport.Routing();
        var router = routing.ConnectToRouter("Switch");
        router.RegisterPublisher(typeof(OrderAccepted), "Red.Sales");

        #endregion

        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Red);
        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Green);

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}