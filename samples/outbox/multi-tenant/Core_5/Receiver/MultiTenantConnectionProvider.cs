using System;
using System.Data;
using NHibernate.Connection;

class MultiTenantConnectionProvider :
    DriverConnectionProvider
{

    public override IDbConnection GetConnection()
    {
        var defaultConnectionString = Connections.Default;

        #region GetConnectionFromContext

        var executor = Program.PipelineExecutor;
        var key = $"LazySqlConnection-{defaultConnectionString}";
        if (
            executor != null &&
            executor.CurrentContext.TryGet(key, out Lazy<IDbConnection> lazy))
        {
            var connection = Driver.CreateConnection();
            connection.ConnectionString = lazy.Value.ConnectionString;
            connection.Open();
            return connection;
        }
        return base.GetConnection();

        #endregion
    }
}