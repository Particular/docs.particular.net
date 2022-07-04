using NServiceBus.Pipeline;
using System.Diagnostics;

#region add-tags-from-outgoing-behavior
class TraceOutgoingOrderIdsBehavior : Behavior<IOutgoingLogicalMessageContext>
{
    public override Task Invoke(
        IOutgoingLogicalMessageContext context,
        Func<Task> next
    )
    {
        if(context.Message.Instance is CreateOrder createOrder)
        {
            Activity.Current?.AddTag("sample.order_id", createOrder.OrderId);
        }
        else if(context.Message.Instance is ShipOrder shipOrder)
        {
            Activity.Current?.AddTag("sample.order_id", shipOrder.OrderId);
        }
        else if(context.Message.Instance is BillOrder billOrder)
        {
            Activity.Current?.AddTag("sample.order_id", billOrder.OrderId);
        }
        return next();
    }
}
#endregion