using System;
using System.Data.SqlClient;
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

        var redSubscriptionStorage = new SqlSubscriptionStorage(() => new SqlConnection(SwitchConnectionString), "Red", new SqlDialect.MsSqlServer(), null);
        await redSubscriptionStorage.Install().ConfigureAwait(false);

        var greenSubscriptionStorage = new SqlSubscriptionStorage(() => new SqlConnection(SwitchConnectionString), "Green", new SqlDialect.MsSqlServer(), null);
        await greenSubscriptionStorage.Install().ConfigureAwait(false);

        #region SwitchConfig

        var routerConfig = new RouterConfiguration("Switch");

        var blueSubscriptionStorage = new SqlSubscriptionStorage(
            () => new SqlConnection(SwitchConnectionString),
            "Blue",
            new SqlDialect.MsSqlServer(), 
            null);
        await blueSubscriptionStorage.Install().ConfigureAwait(false);

        routerConfig.AddInterface<SqlServerTransport>("Blue", t =>
        {
            t.ConnectionString(ConnectionStrings.Blue);
        }).EnableMessageDrivenPublishSubscribe(blueSubscriptionStorage);

        routerConfig.AddInterface<SqlServerTransport>("Red", t =>
        {
            t.ConnectionString(ConnectionStrings.Red);
        }).EnableMessageDrivenPublishSubscribe(redSubscriptionStorage);

        routerConfig.AddInterface<SqlServerTransport>("Green", t =>
        {
            t.ConnectionString(ConnectionStrings.Green);
        }).EnableMessageDrivenPublishSubscribe(greenSubscriptionStorage);

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