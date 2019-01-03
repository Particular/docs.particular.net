using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Router;

static class Program
{
    const string ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=update_and_publish;Integrated Security=True;Max Pool Size=100";

    static async Task Main()
    {
        Console.Title = "Router";

        #region RouterConfig

        var routerConfig = new RouterConfiguration("Samples.Router.UpdateAndPublish.Router");

        var frontendInterface = routerConfig.AddInterface<SqlServerTransport>("SQL", t =>
        {
            t.ConnectionString(ConnectionString);
            t.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
        });
        frontendInterface.EnableMessageDrivenPublishSubscribe(new SqlSubscriptionStorage(() => new SqlConnection(ConnectionString), "", new SqlDialect.MsSqlServer(), null));

        var backendInterface = routerConfig.AddInterface<LearningTransport>("Learning", t => { });

        var staticRouting = routerConfig.UseStaticRoutingProtocol();
        staticRouting.AddForwardRoute("SQL", "Learning");
        staticRouting.AddForwardRoute("Learning", "SQL");

        routerConfig.AutoCreateQueues();

        #endregion

        SqlHelper.EnsureDatabaseExists(ConnectionString);

        var router = Router.Create(routerConfig);

        await router.Start().ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit");
        Console.ReadLine();

        await router.Stop().ConfigureAwait(false);
    }
}