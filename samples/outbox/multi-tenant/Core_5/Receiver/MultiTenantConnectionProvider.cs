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

        var pipelineExecutor = Program.PipelineExecutor;
        var key = $"LazySqlConnection-{defaultConnectionString}";
        if (
            pipelineExecutor != null &&
            pipelineExecutor.CurrentContext.TryGet(key, out var lazy))
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