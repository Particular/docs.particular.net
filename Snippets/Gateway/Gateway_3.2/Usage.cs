using NServiceBus;
using NServiceBus.Gateway;

class Usage
{
    void ReplyUri(EndpointConfiguration endpointConfiguration)
    {
        #region SetReplyToUri

        var gatewaySettings = endpointConfiguration.Gateway();

        // Local HTTP binding address uses wilcard domain
        gatewaySettings.AddReceiveChannel("http://+:12345/MyEndpoint/");

        // Set the reply-to URI as the public address of a load balancer or proxy
        gatewaySettings.SetReplyToUri("http://my-public-domain.com:54321/MyEndpoint/");

        #endregion
    }
}
