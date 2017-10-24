using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;

class Program
{
    static async Task Main()
    {
        //required to prevent possible occurrence of .NET Core issue https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        Console.Title = "Samples.SqlSagaFinder.SqlServer";
        var endpointConfiguration = new EndpointConfiguration("Samples.SqlSagaFinder.SqlServer");
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.EnableInstallers();

        #region sqlServerConfig

        var connection = @"Data Source=.\SqlExpress;Database=NsbSamplesSqlSagaFinder;Integrated Security=True";
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlDialect<SqlDialect.MsSqlServer>();
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connection);
            });
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));

        #endregion

        SqlHelper.EnsureDatabaseExists(connection);
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var startOrder = new StartOrder
        {
            OrderId = "123"
        };
        await endpointInstance.SendLocal(startOrder)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
