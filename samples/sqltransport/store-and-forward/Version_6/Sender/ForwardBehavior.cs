using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus.Extensibility;
using NServiceBus.ObjectBuilder;
using NServiceBus.Pipeline;
using NServiceBus.Routing;
using NServiceBus.Transports;

public class ForwardBehavior : ForkConnector<IIncomingPhysicalMessageContext, IDispatchContext>
{
    public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next, Func<IDispatchContext, Task> fork)
    {
        string destination;
        if (context.Message.Headers.TryGetValue("$.store-and-forward.destination", out destination))
        {
            TransportOperation operation = new TransportOperation(
                new OutgoingMessage(context.MessageId, context.Message.Headers, context.Message.Body),
                new UnicastAddressTag(destination));
            return fork(new DispatchContext(operation, context));

        }
        if (context.Message.Headers.TryGetValue("$.store-and-forward.eventtype", out destination))
        {
            Type messageType = Type.GetType(destination, true);
            TransportOperation operation = new TransportOperation(
                new OutgoingMessage(context.MessageId, context.Message.Headers, context.Message.Body),
                new MulticastAddressTag(messageType));
            return fork(new DispatchContext(operation, context));
        }
        return next();
    }

    class DispatchContext : ContextBag, IDispatchContext
    {
        TransportOperation operation;
        public ContextBag Extensions => this;
        public IBuilder Builder => Get<IBuilder>();

        public DispatchContext(TransportOperation operation, IBehaviorContext parentContext)
            : base(parentContext?.Extensions)
        {
            this.operation = operation;
        }

        public IEnumerable<TransportOperation> Operations
        {
            get { yield return operation; }
        }
    }
}