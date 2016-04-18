using System;
using System.Configuration;
using System.Data;
using NHibernate.Connection;
using NServiceBus.Pipeline;

class MultiTenantConnectionProvider : DriverConnectionProvider
{

    public override IDbConnection GetConnection()
    {
        string defaultConnectionString = ConfigurationManager.ConnectionStrings["NServiceBus/Persistence"]
            .ConnectionString;

        #region GetConnectionFromContext

        Lazy<IDbConnection> lazy;
        PipelineExecutor pipelineExecutor = Program.PipelineExecutor;
        string key = $"LazySqlConnection-{defaultConnectionString}";
        if (pipelineExecutor != null && pipelineExecutor.CurrentContext.TryGet(key, out lazy))
        {
            IDbConnection connection = Driver.CreateConnection();
            connection.ConnectionString = lazy.Value.ConnectionString;
            connection.Open();
            return connection;
        }
        return base.GetConnection();

        #endregion
    }
}