using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Router;

public class RouterConfigurator
{
    static string[] allRouters = { "Red", "Green", "Blue" };
    static bool enableDeduplication = true;
    static bool enableRandomDuplication = true;

    public static async Task<RouterConfiguration> Prepare(string sqlConnectionString, string routerName)
    {
        var otherRouters = allRouters.Except(new[] { routerName }).ToArray();

        var backplaneSubscriptionStorage = new SqlSubscriptionStorage(() => new SqlConnection(sqlConnectionString), $"{routerName}-Backplane", new SqlDialect.MsSqlServer(), null);

        #region RouterConfig

        var routerConfig = new RouterConfiguration(routerName);
        var sqlInterface = routerConfig.AddInterface<SqlServerTransport>("SQL", t =>
        {
            t.ConnectionString(sqlConnectionString);
            t.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
        });

        var backplaneInterface = routerConfig.AddInterface<RabbitMQTransport>("Backplane", t =>
        {
            t.ConnectionString("host=localhost");
            t.UseConventionalRoutingTopology();
        });

        backplaneInterface.EnableMessageDrivenPublishSubscribe(backplaneSubscriptionStorage);
        backplaneInterface.DisableNativePubSub();

        routerConfig.AutoCreateQueues();
#pragma warning disable 618
        routerConfig.ConfigureDeduplication().EnableInstaller(true);
#pragma warning restore 618

        #endregion

        if (enableRandomDuplication)
        {
            //Randomly duplicate messages sent to the backplane
            routerConfig.AddRule(c => new RandomDuplicator(c.Endpoint), c => c.InterfaceName == "Backplane");
        }

        if (enableDeduplication)
        {
            #region Deduplication

            foreach (var router in otherRouters)
            {
                sqlInterface.EnableDeduplication("Backplane", router, 
                    () => new SqlConnection(sqlConnectionString), 10);
            }

            #endregion
        }

        #region RoutingTopology

        var staticRouting = routerConfig.UseStaticRoutingProtocol();

        //Forward messages coming from local SQL based on the endpoint name prefix
        foreach (var router in otherRouters)
        {
            staticRouting.AddRoute(
                destinationFilter: (@interface, dest) =>
                {
                    return @interface == "SQL" 
                           && dest.Endpoint != null 
                           && dest.Endpoint.StartsWith(router);
                }, 
                destinationFilterDescription: $"To {router}", 
                gateway: router, 
                iface: "Backplane");
        }

        //Forward messages coming from backplane to local SQL
        staticRouting.AddRoute((@interface, dest) => @interface == "Backplane", "To local", null, "SQL");

        #endregion

        await backplaneSubscriptionStorage.Install().ConfigureAwait(false);

        return routerConfig;
    }
}