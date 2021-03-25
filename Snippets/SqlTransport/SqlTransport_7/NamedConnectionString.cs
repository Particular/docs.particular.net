using Microsoft.Data.SqlClient;
using NServiceBus;

class NamedConnectionString
{

    void ConnectionString(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-config-connectionstring

        var transport = new SqlServerTransport("Data Source=instance;Initial Catalog=db;Integrated Security=True;Max Pool Size=80");

        #endregion
    }

    void ConnectionFactory(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-custom-connection-factory

        var transport = new SqlServerTransport(
            async cancellationToken =>
            {
                var connection = new SqlConnection("SomeConnectionString");
                try
                {
                    await connection.OpenAsync().ConfigureAwait(false);

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