using System;
using System.Data;
using System.Data.SqlClient;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

class MultiTenantOpenSqlConnectionBehavior :
    IBehavior<IncomingContext>
{

    public void Invoke(IncomingContext context, Action next)
    {
        var defaultConnectionString = Connections.Default;
        #region OpenTenantDatabaseConnection

        string tenant;
        if (!context.PhysicalMessage.Headers.TryGetValue("TenantId", out tenant))
        {
            throw new InvalidOperationException("No tenant id");
        }
        var connectionString = Connections.GetTenant(tenant);
        var lazyConnection = new Lazy<IDbConnection>(() =>
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        });
        var key = $"LazySqlConnection-{defaultConnectionString}";
        context.Set(key, lazyConnection);
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
            context.Remove(key);
        }

        #endregion
    }
}