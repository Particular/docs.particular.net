using NServiceBus;

namespace LambdaFunctions;

public static class EndpointConfiguration
{
    #region EndpointSetup
    public static IAwsLambdaSQSEndpoint Configure() => new AwsLambdaSQSEndpoint(context =>
    {
        var endpointConfiguration = new AwsLambdaSQSEndpointConfiguration("ServerlessEndpoint");
        endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();

        var routing = endpointConfiguration.Transport.Routing();

        routing.RouteToEndpoint(typeof(TriggerMessage), "ServerlessEndpoint");
        routing.RouteToEndpoint(typeof(ResponseMessage), "RegularEndpoint");

        var advanced = endpointConfiguration.AdvancedConfiguration;
        advanced.SendFailedMessagesTo("error");

        return endpointConfiguration;
    });
    #endregion
}