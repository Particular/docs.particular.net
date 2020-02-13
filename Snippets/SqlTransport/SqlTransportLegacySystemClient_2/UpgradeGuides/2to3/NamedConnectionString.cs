using System.Data.SqlClient;
using NServiceBus;

namespace SqlTransport_2.UpgradeGuides._2to3
{
    class NamedConnectionString
    {

        void ConnectionFactory(BusConfiguration busConfiguration)
        {
            #region 2to3-sqlserver-custom-connection-factory

            var transport = busConfiguration.UseTransport<SqlServerTransport>();
            transport.UseCustomSqlConnectionFactory(
                connectionString =>
                {
                    var connection = new SqlConnection(connectionString);
                    try
                    {
                        connection.Open();

                        // custom operations

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