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

public class Startup
{

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var configuration = new PassThroughConfiguration(OpenConnection);
        configuration.SendingCallback(AmendMessage);
        services.AddSqlHttpPassThrough(configuration);
        services.AddMvcCore();
    }

    void AmendMessage(HttpContext context, PassThroughMessage message)
    {
        message.ExtraHeaders = new Dictionary<string, string>
        {
            {"CustomHeader", "CustomHeaderValue"}
        };
    }

    Task<SqlConnection> OpenConnection(CancellationToken cancellation)
    {
        return ConnectionHelpers.OpenConnection(SqlHelper.ConnectionString, cancellation);
    }

    public void Configure(IApplicationBuilder builder)
    {
        builder.UseMiddleware<LogContextMiddleware>();
        builder.AddSqlHttpPassThroughBadRequestMiddleware();
        builder.UseMvc();
    }
}