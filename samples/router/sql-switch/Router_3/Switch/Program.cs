using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Router;

class Program
{
    const string SwitchConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=sqlswitch;Integrated Security=True;Max Pool Size=100";

    static async Task Main()
    {
        Console.Title = "Switch";

        SqlHelper.EnsureDatabaseExists(SwitchConnectionString);
        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Blue);
        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Red);
        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Green);

        #region SwitchConfig

        var routerConfig = new RouterConfiguration("Switch");

        routerConfig.AddInterface<SqlServerTransport>("Blue", t => { t.ConnectionString(ConnectionStrings.Blue); });
        routerConfig.AddInterface<SqlServerTransport>("Red", t => { t.ConnectionString(ConnectionStrings.Red); });
        routerConfig.AddInterface<SqlServerTransport>("Green", t => { t.ConnectionString(ConnectionStrings.Green); });

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