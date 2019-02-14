using System.Data.Common;
using System.Data.SqlClient;

class MultiTenantConnectionFactory 
{
    public static DbConnection GetConnection()
    {
        #region GetConnectionFromContext

        var connectionString = ExtractTenantConnectionStringBehavior.ConnectionStringHolder.Value 
                               ?? Connections.Shared;

        return new SqlConnection(connectionString);

        #endregion
    }
}
