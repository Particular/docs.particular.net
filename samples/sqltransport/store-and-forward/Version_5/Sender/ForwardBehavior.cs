using System;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

public class ForwardBehavior : IBehavior<IncomingContext>
{
    IBus bus;

    public ForwardBehavior(IBus bus)
    {
        this.bus = bus;
    }

    public void Invoke(IncomingContext context, Action next)
    {
        #region ForwardBehavior

        string eventType;
        string destination;
        if (context.IncomingLogicalMessage.Headers.TryGetValue("$.store-and-forward.destination", out destination))
        {
            //We have the ultimate destination in the header, let's send there (via the outbox) and skip the processing.
            bus.Send(destination, context.IncomingLogicalMessage.Instance);
        }
        else if (context.IncomingLogicalMessage.Headers.TryGetValue("$.store-and-forward.eventtype", out eventType))
        {
            //We have an event that needs to be re-published, let's do it and skip the processing.
            bus.Publish(context.IncomingLogicalMessage.Instance);
        }
        else
        {
            //We have a normal message, process it.
            next();
        }

        #endregion
    }

    public class Registration : RegisterStep
    {
        public Registration()
            : base("Forward", typeof(ForwardBehavior), "Forwards the message to the destination.")
        {
            InsertBefore(WellKnownStep.LoadHandlers);
            InsertAfter(WellKnownStep.MutateIncomingMessages);
        }
    }


}