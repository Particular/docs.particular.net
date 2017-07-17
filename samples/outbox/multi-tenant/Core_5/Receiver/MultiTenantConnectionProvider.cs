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

        Lazy<IDbConnection> lazy;
        var pipelineExecutor = Program.PipelineExecutor;
        var key = $"LazySqlConnection-{defaultConnectionString}";
        if (
            pipelineExecutor != null &&
            pipelineExecutor.CurrentContext.TryGet(key, out lazy))
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