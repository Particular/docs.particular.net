using NServiceBus;
using NServiceBus.Transport.SQLServer;

class MessageDrivenPubSubCompatibility
{
    void ConfigureCompatibilityMode(EndpointConfiguration endpointConfiguration)
    {
        #region 4to5-enable-message-driven-pub-sub-compatibility-mode

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        var pubSubCompatibilityMode = transport.EnableMessageDrivenPubSubCompatibilityMode();

        #endregion

        #region 4to5-configure-message-driven-pub-sub-routing

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

        #endregion

        #region 4to5-configure-message-driven-pub-sub-auth

        pubSubCompatibilityMode.SubscriptionAuthorizer(
            incomingMessageContext => true);

        #endregion
       
    }

    class SomeEvent { }
}