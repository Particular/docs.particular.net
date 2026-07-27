using NServiceBus.Pipeline;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

#region capture-message-property-tags

class CaptureOrderTagsBehavior : Behavior<IIncomingLogicalMessageContext>
{
    public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        if (Activity.Current is { IsAllDataRequested: true } activity
            && context.Message.Instance is PlaceOrder order)
        {
            activity.SetTag("sample.order.id", order.OrderId);
            activity.SetTag("sample.order.customer-id", order.CustomerId);
        }
        return next();
    }
}

#endregion
