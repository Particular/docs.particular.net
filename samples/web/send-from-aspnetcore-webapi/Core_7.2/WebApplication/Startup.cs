using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;
public class Startup
{
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

        #region ServiceRegistration

        services.AddNServiceBus(endpointConfiguration);

        #endregion

        services.AddMvc();
        services.AddLogging(loggingBuilder => loggingBuilder.AddDebug());
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseMvc(routeBuilder => routeBuilder.MapRoute(name: "default",
            template: "{controller=SendMessage}/{action=Get}"));
    }
}