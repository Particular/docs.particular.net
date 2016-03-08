using System;
using System.Configuration;
using System.Data;
using NHibernate.Connection;

public class MultiTenantConnectionProvider : DriverConnectionProvider
{
    private static readonly string defaultConnectionString = ConfigurationManager.ConnectionStrings["NServiceBus/Persistence"].ConnectionString;

    public override IDbConnection GetConnection()
    {
        #region GetConnectionFromContext

        Lazy<IDbConnection> lazy;
        if (Program.PipelineExecutor != null && Program.PipelineExecutor.CurrentContext.TryGet($"LazySqlConnection-{defaultConnectionString}", out lazy))
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