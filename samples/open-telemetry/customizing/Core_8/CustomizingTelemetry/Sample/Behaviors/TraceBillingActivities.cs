using NServiceBus.Pipeline;

#region custom-activity-in-behavior
class TraceBillingActivities : Behavior<IIncomingLogicalMessageContext>
{
    public override async Task Invoke(
        IIncomingLogicalMessageContext context,
        Func<Task> next
    )
    {
        if(context.Message.Instance is BillOrder billOrder)
        {
            using var activity = CustomActivitySources.Main.StartActivity(
                "Billing operation"
            );
            activity?.AddTag("sample.billing.order_id", billOrder.OrderId);
            await next();
        }
        else
        {
            await next();
        }
    }
}
#endregion