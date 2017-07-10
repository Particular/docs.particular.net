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
using NServiceBus.Transport;


#region SendThroughLocalQueueBehavior

class SendThroughLocalQueueRoutingToDispatchConnector :
    ForkConnector<IRoutingContext, IDispatchContext>
{
    public SendThroughLocalQueueRoutingToDispatchConnector(string localAddress)
    {
        this.localAddress = localAddress;
    }

    public override Task Invoke(IRoutingContext context, Func<Task> next, Func<IDispatchContext, Task> fork)
    {
        IncomingMessage incomingMessage;
        var fromHandler = context.Extensions.TryGet(out incomingMessage);

        DelayedDeliveryConstraint constraint;
        if (context.Extensions.TryGetDeliveryConstraint(out constraint) || fromHandler)
        {
            return next();
        }
        var operations = context.RoutingStrategies
            .Select(s => RouteThroughLocalEndpointInstance(s, context))
            .ToArray();
        return fork(new DispatchContext(operations, context));
    }

    TransportOperation RouteThroughLocalEndpointInstance(RoutingStrategy routingStrategy, IRoutingContext context)
    {
        var outgoingMessage = context.Message;
        var headers = new Dictionary<string, string>(outgoingMessage.Headers);
        var originalTag = routingStrategy.Apply(headers);
        var unicastTag = originalTag as UnicastAddressTag;
        if (unicastTag == null)
        {
            var multicastTag = originalTag as MulticastAddressTag;
            if (multicastTag == null)
            {
                throw new Exception($"Unsupported type of address tag: {originalTag.GetType().FullName}");
            }
            headers["$.store-and-forward.eventtype"] = multicastTag.MessageType.AssemblyQualifiedName;
        }
        else
        {
            headers["$.store-and-forward.destination"] = unicastTag.Destination;
        }
        var message = new OutgoingMessage(outgoingMessage.MessageId, headers, outgoingMessage.Body);
        return new TransportOperation(
            message: message,
            addressTag: new UnicastAddressTag(localAddress),
            requiredDispatchConsistency: DispatchConsistency.Default,
            deliveryConstraints: context.Extensions.GetDeliveryConstraints());
    }

    string localAddress;

    class DispatchContext :
        ContextBag,
        IDispatchContext
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

#endregion