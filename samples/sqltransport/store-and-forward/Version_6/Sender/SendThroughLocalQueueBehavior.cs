using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus.Pipeline;
using NServiceBus.Routing;
using NServiceBus.Transports;


class SendThroughLocalQueueRoutingToDispatchConnector : Behavior<IRoutingContext>
{
    public SendThroughLocalQueueRoutingToDispatchConnector(string localAddress)
    {
        this.localAddress = localAddress;
    }

    public override Task Invoke(IRoutingContext context, Func<Task> next)
    {
        IncomingMessage incomingMessage;
        bool fromHandler = context.Extensions.TryGet(out incomingMessage);

        context.RoutingStrategies = context.RoutingStrategies.Select(s => fromHandler ? s : RouteThroughLocalEndpointInstance(s, context)).ToArray();

        return next();
    }

    RoutingStrategy RouteThroughLocalEndpointInstance(RoutingStrategy routingStrategy, IRoutingContext context)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        AddressTag originalTag = routingStrategy.Apply(headers);
        if (headers.Any())
        {
            throw new Exception("Cannot use forwarding with messages with custom routing strategies that use message headers.");
        }
        UnicastAddressTag unicastTag = originalTag as UnicastAddressTag;
        if (unicastTag != null)
        {
            context.Message.Headers["$.store-and-forward.destination"] = unicastTag.Destination;
        }
        else
        {
            MulticastAddressTag multicastTag = originalTag as MulticastAddressTag;
            if (multicastTag != null)
            {
                context.Message.Headers["$.store-and-forward.eventtype"] = multicastTag.MessageType.AssemblyQualifiedName;
            }
            else
            {
                throw new Exception("Unsupported type of address tag: " + originalTag.GetType().FullName);
            }
        }
        return new UnicastRoutingStrategy(localAddress);
    }

    string localAddress;
}