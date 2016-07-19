using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

class ExtractTenantConnectionStringBehavior :
    Behavior<ITransportReceiveContext>
{
    internal static AsyncLocal<string> ConnectionStringHolder = new AsyncLocal<string>();

    public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
    {
        var defaultConnectionString = ConfigurationManager.ConnectionStrings["NServiceBus/Persistence"]
            .ConnectionString;

        #region OpenTenantDatabaseConnection

        string tenant;
        if (!context.Message.Headers.TryGetValue("TenantId", out tenant))
        {
            throw new InvalidOperationException("No tenant id");
        }
        var connectionString = ConfigurationManager.ConnectionStrings[tenant]
            .ConnectionString;

        ConnectionStringHolder.Value = connectionString;
        try
        {
            await next()
                .ConfigureAwait(false);
        }
        finally
        {
            ConnectionStringHolder.Value = null;
        }

        #endregion
    }

    public class Registration :
        RegisterStep
    {
        public Registration()
            : base("ExtractTenantConnectionString", typeof(ExtractTenantConnectionStringBehavior), "Extracts tenant connection string based on tenant ID header.")
        {
        }
    }
}