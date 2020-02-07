using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SqlPersistence.EndpointSqlServer";

        #region sqlServerConfig

        var endpointConfiguration = new EndpointConfiguration("Samples.SqlPersistence.EndpointSqlServer");

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var connection = @"Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlPersistence;Integrated Security=True";
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

        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}