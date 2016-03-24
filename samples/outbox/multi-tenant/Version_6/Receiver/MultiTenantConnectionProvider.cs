using System;
using System.Configuration;
using System.Data;
using NHibernate.Connection;
using Receiver;

class MultiTenantConnectionProvider : DriverConnectionProvider
{
    public override IDbConnection GetConnection()
    {
        string defaultConnectionString = ConfigurationManager.ConnectionStrings["NServiceBus/Persistence"]
            .ConnectionString;

        #region GetConnectionFromContext

        Lazy<IDbConnection> lazy;
        PipelineExecution pipelineExecutor = PipelineExecution.Instance;
        string key = $"LazySqlConnection-{defaultConnectionString}";
        if (pipelineExecutor.CurrentContext != null && pipelineExecutor.CurrentContext.Extensions.TryGet(key, out lazy))
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