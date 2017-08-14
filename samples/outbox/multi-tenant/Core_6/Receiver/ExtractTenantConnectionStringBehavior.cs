using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

class ExtractTenantConnectionStringBehavior :
    Behavior<ITransportReceiveContext>
{
    internal static AsyncLocal<string> ConnectionStringHolder = new AsyncLocal<string>();

    public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
    {
        var defaultConnectionString = Connections.Default;

        #region OpenTenantDatabaseConnection

        if (!context.Message.Headers.TryGetValue("TenantId", out var tenant))
        {
            throw new InvalidOperationException("No tenant id");
        }
        var connectionString = Connections.GetTenant(tenant);

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
            : base(
                stepId: "ExtractTenantConnectionString",
                behavior: typeof(ExtractTenantConnectionStringBehavior),
                description: "Extracts tenant connection string based on tenant ID header.")
        {
        }
    }
}