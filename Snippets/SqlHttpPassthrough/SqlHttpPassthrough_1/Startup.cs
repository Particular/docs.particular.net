using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus.SqlServer.HttpPassthrough;

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
        var configuration = new PassThroughConfiguration(OpenConnection);
        services.AddSqlHttpPassThrough(configuration);
        services.AddMvcCore();
        // other ASP.MVC config
    }

    public void Configure(IApplicationBuilder builder)
    {
        builder.AddSqlHttpPassThroughBadRequestMiddleware();
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