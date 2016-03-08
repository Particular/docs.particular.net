using System;
using System.Configuration;
using System.Data;
using NHibernate.Connection;
using NServiceBus.Pipeline;

class MultiTenantConnectionProvider : DriverConnectionProvider
{
    static string defaultConnectionString = ConfigurationManager.ConnectionStrings["NServiceBus/Persistence"].ConnectionString;

    public override IDbConnection GetConnection()
    {
        #region GetConnectionFromContext

        Lazy<IDbConnection> lazy;
        PipelineExecutor pipelineExecutor = Program.PipelineExecutor;
        if (pipelineExecutor != null && pipelineExecutor.CurrentContext.TryGet($"LazySqlConnection-{defaultConnectionString}", out lazy))
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