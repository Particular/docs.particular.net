using NServiceBus.Pipeline;
using System.Diagnostics;

#region add-tags-from-behavior
class AddOrderIdToTrace : Behavior<IIncomingLogicalMessageContext>
{
    public override Task Invoke(
        IIncomingLogicalMessageContext context,
        Func<Task> next
    )
    {
        if(context.Message.Instance is ShipOrder shipOrder)
        {
            Activity.Current?.AddTag("sample.shipping.order_id", shipOrder.OrderId);
        }

        return next();
    }
}
#endregion