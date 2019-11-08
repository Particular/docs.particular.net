using NServiceBus;

class MessageDrivenPubSub
{
    void Configure(EndpointConfiguration endpointConfiguration)
    {
        #region 4to5-configure-message-driven-pub-sub-routing

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        var routing = transport.Routing();

        routing.RegisterPublisher(
            eventType: typeof(SomeEvent), 
            publisherEndpoint: "PublisherEndpoint");

        routing.RegisterPublisher(
            assembly: typeof(SomeEvent).Assembly, 
            publisherEndpoint: "PublisherEndpoint");

        routing.RegisterPublisher(
            assembly: typeof(SomeEvent).Assembly, 
            @namespace: "Namespace", 
            publisherEndpoint: "PublisherEndpoint");

        #endregion

        #region 4to5-configure-message-driven-pub-sub-auth

        transport.SubscriptionAuthorizer(
            incomingMessageContext => true);

        #endregion
    }

    class SomeEvent { }
}