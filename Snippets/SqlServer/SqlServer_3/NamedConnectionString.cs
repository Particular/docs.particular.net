using System.Data.SqlClient;
using NServiceBus;
using NServiceBus.Transport.SQLServer;

class NamedConnectionString
{

    void ConnectionString(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-config-connectionstring

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(
            "Data Source=INSTANCE_NAME;Initial Catalog=some_database;Integrated Security=True");

        #endregion
    }

    void ConnectionName(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-named-connection-string

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionStringName("MyConnectionString");

        #endregion
    }

    void ConnectionFactory(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-custom-connection-factory

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.UseCustomSqlConnectionFactory(
            sqlConnectionFactory: async () =>
        {
            var connection = new SqlConnection("SomeConnectionString");
            try
            {
                await connection.OpenAsync()
                    .ConfigureAwait(false);

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
