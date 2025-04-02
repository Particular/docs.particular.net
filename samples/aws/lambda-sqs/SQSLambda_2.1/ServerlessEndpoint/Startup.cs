using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.AwsLambda.SQS;

namespace LambdaFunctions;

[Amazon.Lambda.Annotations.LambdaStartup]
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAwsLambdaNServiceBusEndpoint("ServerlessEndpoint", (endpointConfiguration, _) =>
        {
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();

            var routing = endpointConfiguration.RoutingSettings;

            routing.RouteToEndpoint(typeof(TriggerMessage), "ServerlessEndpoint");
            routing.RouteToEndpoint(typeof(ResponseMessage), "RegularEndpoint");
        });
    }
}