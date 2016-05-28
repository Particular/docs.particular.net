using System.Data;
using NHibernate.Connection;

class MultiTenantConnectionProvider : DriverConnectionProvider
{
    public override IDbConnection GetConnection()
    {
        #region GetConnectionFromContext

        var connectionString = ExtractTenantConnectionStringBehavior.ConnectionStringHolder.Value;
        if (connectionString != null)
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
