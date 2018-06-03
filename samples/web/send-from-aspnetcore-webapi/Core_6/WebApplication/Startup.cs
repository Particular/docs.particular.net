using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class Startup
{
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

        var endpointConfiguration = new EndpointConfiguration("Samples.ASPNETCore.Sender");
        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.SendOnly();

        #endregion

        #region Routing

        var routing = transport.Routing();
        routing.RouteToEndpoint(
            assembly: typeof(MyMessage).Assembly,
            destination: "Samples.ASPNETCore.Endpoint");

        #endregion

        #region EndpointStart

        endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

        #endregion

        #region ServiceRegistration

        services.AddSingleton<IMessageSession>(endpoint);

        #endregion

        services.AddMvc();
    }


    public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime, ILoggerFactory loggerFactory)
    {
        loggerFactory.AddDebug();

        applicationLifetime.ApplicationStopping.Register(OnShutdown);

        app.UseMvc();
    }

    void OnShutdown()
    {
        endpoint?.Stop().GetAwaiter().GetResult();
    }

    IEndpointInstance endpoint;
}