using NServiceBus.Pipeline;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

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