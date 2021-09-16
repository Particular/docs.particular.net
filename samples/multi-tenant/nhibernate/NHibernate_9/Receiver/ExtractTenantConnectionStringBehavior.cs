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
        var defaultConnectionString = Connections.Shared;

        #region PutConnectionStringToContext

        if (!context.Message.Headers.TryGetValue("tenant_id", out var tenant))
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
}