using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

class ExtractTenantConnectionStringBehavior :
    Behavior<ITransportReceiveContext>
{
    internal static AsyncLocal<string> ConnectionStringHolder = new();

    public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
    {
        #region PutConnectionStringToContext

        if (!context.Message.Headers.TryGetValue("tenant_id", out var tenant))
        {
            throw new InvalidOperationException("No tenant id");
        }

        Console.WriteLine($"Setting connection for tenant {tenant}");

        var connectionString = Connections.GetForTenant(tenant);

        ConnectionStringHolder.Value = connectionString;
        try
        {
            await next();
        }
        finally
        {
            ConnectionStringHolder.Value = null;
        }

        #endregion
    }
}