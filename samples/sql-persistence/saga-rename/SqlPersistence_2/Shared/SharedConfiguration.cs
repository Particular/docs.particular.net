using System;
using System.Data.SqlClient;
using NServiceBus;
using NServiceBus.Persistence.Sql;

public static class SharedConfiguration
{

    public static void Apply(EndpointConfiguration endpointConfiguration)
    {
        #region endpointConfig

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlVariant(SqlVariant.MsSqlServer);
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));
        var connection = @"Data Source=.\SQLEXPRESS;Initial Catalog=SqlPersistenceSample;Integrated Security=True";
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connection);
            });

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseTransport<LearningTransport>();

        #endregion
    }
}