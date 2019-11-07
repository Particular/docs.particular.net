using NServiceBus;
using NServiceBus.Transport.SQLServer;

class ConfigureMessageDrivenPubSubRouting
{
    public ConfigureMessageDrivenPubSubRouting(EndpointConfiguration endpointConfiguration)
    {
        #region 4to5-configure-message-driven-pub-sub-routing

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        var pubSubCompatibilityMode = transport.EnableMessageDrivenPubSubCompatibilityMode();

        pubSubCompatibilityMode.RegisterPublisher(
            eventType: typeof(SomeEvent), 
            publisherEndpoint: "PublisherEndpoint");

        pubSubCompatibilityMode.RegisterPublisher(
            assembly: typeof(SomeEvent).Assembly, 
            publisherEndpoint: "PublisherEndpoint");

        pubSubCompatibilityMode.RegisterPublisher(
            assembly: typeof(SomeEvent).Assembly, 
            @namespace: "Namespace", 
            publisherEndpoint: "PublisherEndpoint");

        pubSubCompatibilityMode.SubscriptionAuthorizer(
            incomingMessageContext => true);

        #endregion
    }

    class SomeEvent { }
}