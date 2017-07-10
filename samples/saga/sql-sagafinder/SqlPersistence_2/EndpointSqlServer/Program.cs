using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.SqlSagaFinder.SqlServer";
        var endpointConfiguration = new EndpointConfiguration("Samples.SqlSagaFinder.SqlServer");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region sqlServerConfig

        var connection = @"Data Source=.\SqlExpress;Database=Samples.Sql.SagaFinder;Integrated Security=True";
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connection);
            });
        persistence.SqlVariant(SqlVariant.MsSqlServer);
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));

        #endregion

        await SqlHelper.EnsureDatabaseExists(connection)
            .ConfigureAwait(false);
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
