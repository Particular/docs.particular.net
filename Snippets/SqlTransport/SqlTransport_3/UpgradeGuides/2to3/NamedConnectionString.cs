using System.Data.SqlClient;
using NServiceBus;
using NServiceBus.Transport.SQLServer;

namespace SqlTransport_3.UpgradeGuides._2to3
{
    class NamedConnectionString
    {

        void ConnectionFactory(EndpointConfiguration endpointConfiguration)
        {
            #region 2to3-sqlserver-custom-connection-factory

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
}