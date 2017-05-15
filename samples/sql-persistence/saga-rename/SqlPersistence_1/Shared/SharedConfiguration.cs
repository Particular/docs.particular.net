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
        var connection = @"Data Source=.\SQLEXPRESS;Initial Catalog=SqlPersistenceSample;Integrated Security=True";
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connection);
            });

        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();

        #endregion
    }
}