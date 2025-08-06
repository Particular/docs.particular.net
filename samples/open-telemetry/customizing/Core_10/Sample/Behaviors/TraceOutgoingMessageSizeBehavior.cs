using NServiceBus.Pipeline;
using System.Diagnostics;

#region add-tags-from-outgoing-behavior

class TraceOutgoingMessageSizeBehavior : Behavior<IOutgoingPhysicalMessageContext>
{
    public override Task Invoke(IOutgoingPhysicalMessageContext context, Func<Task> next)
    {
        Activity.Current?.AddTag("sample.messaging.body.size", context.Body.Length);
        return next();
    }
}

#endregion