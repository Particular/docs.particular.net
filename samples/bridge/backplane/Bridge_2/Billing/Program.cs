using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Billing";
        var endpointConfiguration = new EndpointConfiguration("Billing");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(ConnectionStrings.Green);

        #region BillingBridgeRouting

        var routing = transport.Routing();
        var bridge = routing.ConnectToBridge("Green");
        bridge.RegisterPublisher(typeof(OrderAccepted), "Sales");

        #endregion

        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Green);

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}