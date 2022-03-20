using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Router;

class Program
{
    const string SwitchConnectionString = @"Data Source=(local);Initial Catalog=sqlswitch;Integrated Security=True;Max Pool Size=100";

    static async Task Main()
    {
        Console.Title = "Switch";

        SqlHelper.EnsureDatabaseExists(SwitchConnectionString);
        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Blue);

        #region SwitchConfig

        var routerConfig = new RouterConfiguration("Switch");

        routerConfig.AddInterface<SqlServerTransport>("Blue",
            t =>
            {
                t.Transactions(TransportTransactionMode.ReceiveOnly);
                t.ConnectionString(ConnectionStrings.Blue);
            });
        var redInterface = routerConfig.AddInterface<AzureServiceBusTransport>("Red",
            t =>
            {
                t.Transactions(TransportTransactionMode.ReceiveOnly);
                t.ConnectionString(ConnectionStrings.Red);
                t.TopicName("bundle-red");
            });
        redInterface.OverrideEndpointName("Switch-Red");

        var greenInterface = routerConfig.AddInterface<AzureServiceBusTransport>("Green",
            t =>
            {
                t.Transactions(TransportTransactionMode.ReceiveOnly);
                t.ConnectionString(ConnectionStrings.Green);
                t.TopicName("bundle-green");
            });
        greenInterface.OverrideEndpointName("Switch-Green");

        routerConfig.AutoCreateQueues();

        #endregion

        #region SwitchForwarding

        var staticRouting = routerConfig.UseStaticRoutingProtocol();
        //Send all messages to endpoints which name starts with Sales via interface A
        staticRouting.AddRoute(
            destinationFilter: (iface, destination) => destination.Endpoint.StartsWith("Red."),
            destinationFilterDescription: "To Red",
            gateway: null,
            iface: "Red");

        staticRouting.AddRoute(
            (iface, destination) => destination.Endpoint.StartsWith("Blue."),
            "To Blue", null, "Blue");

        staticRouting.AddRoute(
            (iface, destination) => destination.Endpoint.StartsWith("Green."),
            "To Green", null, "Green");

        #endregion

        var router = Router.Create(routerConfig);

        await router.Start().ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit");
        Console.ReadLine();

        await router.Stop().ConfigureAwait(false);
    }
}