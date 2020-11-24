using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Pipeline;

#region BehaviorUsingHeader
class OrderIdHeaderAsPartitionKeyBehavior : Behavior<ITransportReceiveContext>
{
    public override Task Invoke(ITransportReceiveContext context, Func<Task> next)
    {
        if (context.Message.Headers.TryGetValue("Sample.AzureTable.Transaction.OrderId", out var orderId))
        {
            Log.Info($"Found partition key '{orderId}' from header 'Sample.AzureTable.Transaction'");

            context.Extensions.Set(new TableEntityPartitionKey(orderId));
        }

        return next();
    }

    static readonly ILog Log = LogManager.GetLogger<OrderIdHeaderAsPartitionKeyBehavior>();
}
#endregion