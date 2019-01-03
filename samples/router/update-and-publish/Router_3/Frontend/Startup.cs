using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class Startup
{
    const string ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=update_and_publish;Integrated Security=True;Max Pool Size=100";

    public Startup(IHostingEnvironment env)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddEnvironmentVariables();
        Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        #region EndpointConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.Router.UpdateAndPublish.Frontend");

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(ConnectionString);
        transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();

        #endregion

        SqlHelper.EnsureDatabaseExists(ConnectionString);

        endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

        services.AddSingleton<IMessageSession>(endpoint);
        services.AddSingleton<Func<SqlConnection>>(() => new SqlConnection(ConnectionString));

        services.AddMvc();
    }


    public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime, ILoggerFactory loggerFactory)
    {
        applicationLifetime.ApplicationStopping.Register(OnShutdown);

        app.UseMvc(routeBuilder => routeBuilder.MapRoute(name: "default",
            template: "{controller=AcceptOrder}/{action=Post}"));
    }

    void OnShutdown()
    {
        endpoint?.Stop().GetAwaiter().GetResult();
    }

    IEndpointInstance endpoint;
}