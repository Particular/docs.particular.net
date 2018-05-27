using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus.SqlServer.HttpPassthrough;
using NServiceBus.Transport.SqlServerNative;

#region Startup
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var configuration = new PassthroughConfiguration(OpenConnection, AmendMessage);
        services.AddSqlHttpPassthrough(configuration);
        services.AddMvcCore();
    }

    Task<Table> AmendMessage(HttpContext context, PassthroughMessage message)
    {
        message.ExtraHeaders = new Dictionary<string, string>
        {
            {"CustomHeader", "CustomHeaderValue"}
        };
        //TODO: validate that the destination allowed
        var destinationTable = new Table(message.Destination);
        return Task.FromResult(destinationTable);
    }

    public void Configure(IApplicationBuilder builder)
    {
        builder.AddSqlHttpPassthroughBadRequestMiddleware();
        builder.UseMvc();
    }

    Task<SqlConnection> OpenConnection(CancellationToken cancellation)
    {
        return ConnectionHelpers.OpenConnection(SqlHelper.ConnectionString, cancellation);
    }
}
#endregion