using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus.SqlServer.HttpPassthrough;
using NServiceBus.Transport.SqlServerNative;

public class AppendClaimsToMessageHeaders
{

    public void WithPrefix(IServiceCollection services)
    {
        #region AppendClaimsToMessageHeaders_WithPrefix

        var configuration = new PassthroughConfiguration(
            connectionFunc: OpenConnection,
            callback: Callback,
            dedupCriticalError: exception =>
            {
                Environment.FailFast("Dedup cleanup failure", exception);
            });
        configuration.AppendClaimsToMessageHeaders(headerPrefix: "Claim.");
        services.AddSqlHttpPassthrough(configuration);

        #endregion
    }

    public void Default(IServiceCollection services)
    {
        #region AppendClaimsToMessageHeaders

        var configuration = new PassthroughConfiguration(
            connectionFunc: OpenConnection,
            callback: Callback,
            dedupCriticalError: exception =>
            {
                Environment.FailFast("Dedup cleanup failure", exception);
            });
        configuration.AppendClaimsToMessageHeaders();
        services.AddSqlHttpPassthrough(configuration);

        #endregion
    }

    Task<Table> Callback(HttpContext httpContext, PassthroughMessage passthroughMessage)
    {
        return null;
    }

    Task<SqlConnection> OpenConnection(CancellationToken cancellation)
    {
        return null;
    }
}