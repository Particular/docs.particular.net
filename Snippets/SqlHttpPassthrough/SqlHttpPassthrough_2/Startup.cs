using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus.SqlServer.HttpPassthrough;
using NServiceBus.Transport.SqlServerNative;

#region Startup

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var configuration = new PassthroughConfiguration(
            connectionFunc: OpenConnection,
            callback: Callback,
            dedupCriticalError: exception =>
            {
                Environment.FailFast("Dedup cleanup failure", exception);
            });
        services.AddSqlHttpPassthrough(configuration);
        services.AddMvcCore();
        // other ASP.MVC config
    }

    Task<Table> Callback(HttpContext httpContext, PassthroughMessage passthroughMessage)
    {
        //TODO: validate that the message type allowed
        //TODO: validate that the destination allowed
        var destinationTable = new Table(passthroughMessage.Destination);
        return Task.FromResult(destinationTable);
    }

    public void Configure(IApplicationBuilder builder)
    {
        builder.AddSqlHttpPassthroughBadRequestMiddleware();
        builder.UseMvc();
        // other ASP.MVC config
    }

    Task<SqlConnection> OpenConnection(CancellationToken cancellation)
    {
        //TODO open and return a SqlConnection
        return null;
    }
}

#endregion