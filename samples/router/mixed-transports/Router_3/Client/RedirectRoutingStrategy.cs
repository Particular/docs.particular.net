using System.Collections.Generic;
using NServiceBus.Routing;

class RedirectRoutingStrategy : RoutingStrategy
{
    private RoutingStrategy original;

    public RedirectRoutingStrategy(RoutingStrategy original)
    {
        this.original = original;
    }

    public override AddressTag Apply(Dictionary<string, string> headers)
    {
        var tag = original.Apply(headers);
        if (tag is MulticastAddressTag)
        {
            //Do nothing. The event is going to be re-published on the other side
        }
        else if (tag is UnicastAddressTag uni)
        {
            //Capture the actual destination as a header. This header is used by the router to decide where to send messages
            headers["NServiceBus.Bridge.DestinationEndpoint"] = uni.Destination;
        }
        return new UnicastAddressTag("Samples.Router.TrafficMirroring.Router");
    }
}