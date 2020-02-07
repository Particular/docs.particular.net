using System;
using Microsoft.Data.SqlClient;
using NServiceBus;

public static class SharedConfiguration
{

    public static void Apply(EndpointConfiguration endpointConfiguration)
    {
        #region endpointConfig

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlDialect<SqlDialect.MsSqlServer>();
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));
        var connection = @"Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlPersistenceRenameSaga;Integrated Security=True";
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connection);
            });

        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseTransport<LearningTransport>();

        #endregion

        SqlHelper.EnsureDatabaseExists(connection);
    }
}