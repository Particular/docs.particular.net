using NServiceBus;

namespace LambdaFunctions;

public static class Endpoint
{
    #region EndpointSetup
    public static IAwsLambdaSQSEndpoint Configuration => new AwsLambdaSQSEndpoint(context =>
    {
        var endpointConfiguration = new AwsLambdaSQSEndpointConfiguration("ServerlessEndpoint");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        var routing = endpointConfiguration.RoutingSettings;

        routing.RouteToEndpoint(typeof(TriggerMessage), "ServerlessEndpoint");
        routing.RouteToEndpoint(typeof(ResponseMessage), "RegularEndpoint");

        return endpointConfiguration;
    });
    #endregion
}