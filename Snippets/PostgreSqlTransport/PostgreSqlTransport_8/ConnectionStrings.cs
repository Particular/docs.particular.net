using Npgsql;
using NServiceBus;

class ConnectionStrings
{
    void ConnectionString(EndpointConfiguration endpointConfiguration)
    {
        #region postgresql-config-connectionstring

        var transport = new PostgreSqlTransport("SomeConnectionString");

        #endregion
    }

    void ConnectionFactory(EndpointConfiguration endpointConfiguration)
    {
        #region postgresql-custom-connection-factory

        var transport = new PostgreSqlTransport(
            async cancellationToken =>
            {
                var connection = new NpgsqlConnection("SomeConnectionString");
                try
                {
                    await connection.OpenAsync();

                    // perform custom operations

                    return connection;
                }
                catch
                {
                    connection.Dispose();
                    throw;
                }
            });

        #endregion
    }
}