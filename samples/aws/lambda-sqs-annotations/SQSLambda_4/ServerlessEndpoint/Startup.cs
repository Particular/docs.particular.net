using Microsoft.Extensions.DependencyInjection;

namespace LambdaFunctions;

#region EndpointSetup
[Amazon.Lambda.Annotations.LambdaStartup]
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAwsLambdaSQSEndpoint("ServerlessEndpoint", (endpointConfiguration, _) =>
        {
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();

            var routing = endpointConfiguration.RoutingSettings;

            routing.RouteToEndpoint(typeof(TriggerMessage), "ServerlessEndpoint");
            routing.RouteToEndpoint(typeof(ResponseMessage), "RegularEndpoint");
        });
    }
}
#endregion