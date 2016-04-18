using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus.DelayedDelivery;
using NServiceBus.DeliveryConstraints;
using NServiceBus.Extensibility;
using NServiceBus.ObjectBuilder;
using NServiceBus.Pipeline;
using NServiceBus.Routing;
using NServiceBus.Transports;


class SendThroughLocalQueueRoutingToDispatchConnector : ForkConnector<IRoutingContext, IDispatchContext>
{
    public SendThroughLocalQueueRoutingToDispatchConnector(string localAddress)
    {
        this.localAddress = localAddress;
    }

    public override Task Invoke(IRoutingContext context, Func<Task> next, Func<IDispatchContext, Task> fork)
    {
        IncomingMessage incomingMessage;
        bool fromHandler = context.Extensions.TryGet(out incomingMessage);

        DelayedDeliveryConstraint constraint;
        if (context.Extensions.TryGetDeliveryConstraint(out constraint) || fromHandler)
        {
            return next();
        }
        TransportOperation[] operations = context.RoutingStrategies
            .Select(s => RouteThroughLocalEndpointInstance(s, context))
            .ToArray();
        return fork(new DispatchContext(operations, context));
    }

    TransportOperation RouteThroughLocalEndpointInstance(RoutingStrategy routingStrategy, IRoutingContext context)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>(context.Message.Headers);
        AddressTag originalTag = routingStrategy.Apply(headers);
        UnicastAddressTag unicastTag = originalTag as UnicastAddressTag;
        if (unicastTag == null)
        {
            MulticastAddressTag multicastTag = originalTag as MulticastAddressTag;
            if (multicastTag != null)
            {
                headers["$.store-and-forward.eventtype"] = multicastTag.MessageType.AssemblyQualifiedName;
            }
            else
            {
                throw new Exception("Unsupported type of address tag: " + originalTag.GetType().FullName);
            }
        }
        else
        {
            headers["$.store-and-forward.destination"] = unicastTag.Destination;
        }
        OutgoingMessage message = new OutgoingMessage(context.Message.MessageId, headers, context.Message.Body);
        return new TransportOperation(message, new UnicastAddressTag(localAddress), DispatchConsistency.Default, context.Extensions.GetDeliveryConstraints());
    }

    string localAddress;

    class DispatchContext : ContextBag, IDispatchContext
    {
        TransportOperation[] operations;
        public ContextBag Extensions => this;
        public IBuilder Builder => Get<IBuilder>();

        public DispatchContext(TransportOperation[] operations, IBehaviorContext parentContext)
            : base(parentContext?.Extensions)
        {
            this.operations = operations;
        }

        public IEnumerable<TransportOperation> Operations => operations;
    }
}