using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Pipeline;

public class OutboxLoopbackReceiveBehavior : Behavior<IIncomingLogicalMessageContext>
{
    public override async Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        #region OutboxLoopbackReceiveBehavior

        string eventType;
        string destination;
        if (context.Headers.TryGetValue("$.store-and-forward.destination", out destination))
        {
            //We have the ultimate destination in the header, let's send there (via the outbox) and skip the processing.
            await context.Send(destination, context.Message.Instance);
        }
        else if (context.Headers.TryGetValue("$.store-and-forward.eventtype", out eventType))
        {
            //We have an event that needs to be re-published, let's do it and skip the processing.
            await context.Publish(context.Message.Instance);
        }
        else
        {
            //We have a normal message, process it.
            await next().ConfigureAwait(false);
        }

        #endregion
    }

    public class Registration : RegisterStep
    {
        public Registration()
            : base("OutboxLoopbackReceive", typeof(OutboxLoopbackReceiveBehavior), "OutboxLoopbackReceive")
        {
            InsertAfter(WellKnownStep.MutateIncomingMessages);
        }
    }
}