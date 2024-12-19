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

        // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlPersistenceRenameSaga;Integrated Security=True;Encrypt=false
        var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlPersistenceRenameSaga;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";

        persistence.ConnectionBuilder(
            () => new SqlConnection(connectionString));

        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());

        #endregion

        SqlHelper.EnsureDatabaseExists(connectionString);
    }
}
