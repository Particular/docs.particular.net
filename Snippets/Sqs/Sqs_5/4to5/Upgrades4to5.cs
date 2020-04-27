using NServiceBus;

class MessageDrivenPubSub
{
    void Configure(EndpointConfiguration endpointConfiguration)
    {
        #region 4to5-enable-message-driven-pub-sub-compatibility-mode

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
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
    }

    class SomeEvent { }
}