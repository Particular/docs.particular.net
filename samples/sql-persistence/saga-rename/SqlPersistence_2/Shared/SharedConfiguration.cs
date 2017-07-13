using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;

public static class SharedConfiguration
{

    public static async Task Apply(EndpointConfiguration endpointConfiguration)
    {
        #region endpointConfig

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlVariant(SqlVariant.MsSqlServer);
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));
        var connection = @"Data Source=.\SqlExpress;Initial Catalog=Samples.SqlPersistence.RenameSaga;Integrated Security=True";
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connection);
            });

        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseTransport<LearningTransport>();

        #endregion

        await SqlHelper.EnsureDatabaseExists(connection)
            .ConfigureAwait(false);
    }
}