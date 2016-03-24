using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus.Pipeline;
using Receiver;

class MultiTenantOpenSqlConnectionBehavior : Behavior<IIncomingPhysicalMessageContext>
{
    public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
    {
        string defaultConnectionString = ConfigurationManager.ConnectionStrings["NServiceBus/Persistence"]
            .ConnectionString;
        #region OpenTenantDatabaseConnection

        PipelineExecution.Instance.CurrentContext = context;

        string tenant;
        if (!context.Message.Headers.TryGetValue("TenantId", out tenant))
        {
            throw new InvalidOperationException("No tenant id");
        }
        string connectionString = ConfigurationManager.ConnectionStrings[tenant]
            .ConnectionString;
        Lazy<IDbConnection> lazyConnection = new Lazy<IDbConnection>(() =>
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        });
        string key = $"LazySqlConnection-{defaultConnectionString}";
        context.Extensions.Set(key, lazyConnection);
        try
        {
            await next().ConfigureAwait(false);
        }
        finally
        {
            if (lazyConnection.IsValueCreated)
            {
                lazyConnection.Value.Dispose();
            }

            context.Extensions.Remove(key);
        }

        #endregion
    }

    public class Registration : RegisterStep
    {
        public Registration()
            : base("OpenSqlConnection", typeof(MultiTenantOpenSqlConnectionBehavior), "Creates a new connnection for the tenant based on the Id on the header.")
        {
            InsertBefore("ExecuteUnitOfWork");
        }
    }
}