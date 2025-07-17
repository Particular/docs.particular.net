using System.Data.Common;
using NHibernate.Connection;

class MultiTenantConnectionProvider :
    DriverConnectionProvider
{
    public override DbConnection GetConnection()
    {
        #region GetConnectionFromContext

        var connectionString = ExtractTenantConnectionStringBehavior.ConnectionStringHolder.Value;
        if (!string.IsNullOrEmpty(connectionString))
        {
            var connection = Driver.CreateConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            return connection;
        }

        #endregion

        return base.GetConnection();
    }
}
