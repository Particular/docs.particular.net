using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus.Extensibility;
using NServiceBus.ObjectBuilder;
using NServiceBus.Pipeline;
using NServiceBus.Routing;
using NServiceBus.Transport;

public class ForwardBehavior :
    ForkConnector<IIncomingPhysicalMessageContext, IDispatchContext>
{
    public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next, Func<IDispatchContext, Task> fork)
    {
        #region ForwardBehavior
        string destination;
        var message = context.Message;
        var headers = message.Headers;
        var body = message.Body;
        if (headers.TryGetValue("$.store-and-forward.destination", out destination))
        {
            var operation = new TransportOperation(
                message: new OutgoingMessage(context.MessageId, headers, body),
                addressTag: new UnicastAddressTag(destination));
            return fork(new DispatchContext(operation, context));

        }
        if (headers.TryGetValue("$.store-and-forward.eventtype", out destination))
        {
            var messageType = Type.GetType(destination, true);
            var operation = new TransportOperation(
                message: new OutgoingMessage(context.MessageId, headers, body),
                addressTag: new MulticastAddressTag(messageType));
            return fork(new DispatchContext(operation, context));
        }
        return next();
        #endregion
    }

    class DispatchContext :
        ContextBag,
        IDispatchContext
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
            get
            {
                yield return operation;
            }
        }
    }
}