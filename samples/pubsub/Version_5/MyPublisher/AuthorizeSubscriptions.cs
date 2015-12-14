using System.Collections.Generic;
using NServiceBus;

class AuthorizeSubscriptions : IAuthorizeSubscriptions
{

    public bool AuthorizeSubscribe(string messageType, string clientEndpoint, IDictionary<string, string> headers)
    {
        string lowerEndpointName = clientEndpoint.ToLowerInvariant();
        return lowerEndpointName.StartsWith("samples.pubsub.subscriber1") ||
               lowerEndpointName.StartsWith("samples.pubsub.subscriber2");
    }

    public bool AuthorizeUnsubscribe(string messageType, string clientEndpoint, IDictionary<string, string> headers)
    {
        return true;
    }
}