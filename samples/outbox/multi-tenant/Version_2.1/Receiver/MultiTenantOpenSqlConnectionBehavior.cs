using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

class MultiTenantOpenSqlConnectionBehavior : IBehavior<IncomingContext>
{
    private static readonly string defaultConnectionString = ConfigurationManager.ConnectionStrings["NServiceBus/Persistence"].ConnectionString;

    public void Invoke(IncomingContext context, Action next)
    {
        #region OpenTenantDatabaseConnection

        string tenant;
        if (!context.PhysicalMessage.Headers.TryGetValue("TenantId", out tenant))
        {
            throw new InvalidOperationException("No tenant id");
        }
        string connectionString = ConfigurationManager.ConnectionStrings[tenant].ConnectionString;
        Lazy<IDbConnection> lazyConnection = new Lazy<IDbConnection>(() =>
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        });
        context.Set($"LazySqlConnection-{defaultConnectionString}", lazyConnection);
        try
        {
            next();
        }
        finally
        {
            if (lazyConnection.IsValueCreated)
            {
                lazyConnection.Value.Dispose();
            }

            context.Remove($"LazySqlConnection-{defaultConnectionString}");
        }

        #endregion
    }
}