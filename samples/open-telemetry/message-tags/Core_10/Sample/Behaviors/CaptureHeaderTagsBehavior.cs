using NServiceBus.Pipeline;
using System.Diagnostics;

#region capture-header-tags

class CaptureHeaderTagsBehavior : Behavior<IIncomingPhysicalMessageContext>
{
    public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
    {
        if (Activity.Current is { IsAllDataRequested: true } activity
            && context.MessageHeaders.TryGetValue("sample.order.priority", out var priority))
        {
            activity.SetTag("sample.order.priority", priority);
        }
        return next();
    }
}

#endregion
