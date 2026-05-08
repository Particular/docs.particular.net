using System.Linq;
using Microsoft.Data.SqlClient;
using NServiceBus;

class AzureSqlFailover
{
    void ConnectionFactoryWithPoolClearing()
    {
        var connectionString = "SomeConnectionString";

        #region sqlserver-azure-failover-connection-factory

        var transport = new SqlServerTransport(
            async cancellationToken =>
            {
                var connection = new SqlConnection(connectionString);
                try
                {
                    await connection.OpenAsync(cancellationToken);
                    return connection;
                }
                catch (SqlException ex) when (IsTransientAzureSqlError(ex))
                {
                    SqlConnection.ClearAllPools();
                    await connection.DisposeAsync();
                    throw;
                }
                catch
                {
                    await connection.DisposeAsync();
                    throw;
                }
            });

        #endregion
    }

    void ConnectionFactoryWithSinglePoolClearing()
    {
        var connectionString = "SomeConnectionString";

        #region sqlserver-azure-failover-single-pool

        var transport = new SqlServerTransport(
            async cancellationToken =>
            {
                var connection = new SqlConnection(connectionString);
                try
                {
                    await connection.OpenAsync(cancellationToken);
                    return connection;
                }
                catch (SqlException ex) when (IsTransientAzureSqlError(ex))
                {
                    SqlConnection.ClearPool(connection);
                    await connection.DisposeAsync();
                    throw;
                }
                catch
                {
                    await connection.DisposeAsync();
                    throw;
                }
            });

        #endregion
    }

    #region sqlserver-azure-failover-transient-errors

    static bool IsTransientAzureSqlError(SqlException ex)
    {
        // 18456 = Login failed (common with Managed Identity after failover)
        // 233   = Connection was closed by the remote host
        // 64    = Named pipe connection failed
        // 4060  = Cannot open database (during failover)
        // 40197 = Error processing request during Azure SQL reconfiguration
        // 40613 = Database not currently available (Azure SQL)
        // 40143 = Connection could not be initialized (Azure SQL)
        // 40540 = Service goal prevented processing the request (Azure SQL)
        int[] transientErrors = [18456, 233, 64, 4060, 40197, 40613, 40143, 40540];
        return ex.Errors.Cast<SqlError>().Any(e => transientErrors.Contains(e.Number));
    }

    #endregion
}
