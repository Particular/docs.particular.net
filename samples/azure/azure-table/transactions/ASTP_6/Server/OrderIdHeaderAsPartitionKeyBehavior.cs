using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Pipeline;

#region BehaviorUsingHeader
class OrderIdHeaderAsPartitionKeyBehavior(ILogger<OrderIdHeaderAsPartitionKeyBehavior> logger) : Behavior<ITransportReceiveContext>
{
    public override Task Invoke(ITransportReceiveContext context, Func<Task> next)
    {
        if (context.Message.Headers.TryGetValue("Sample.AzureTable.Transaction.OrderId", out var orderId))
        {
            logger.LogInformation($"Found partition key '{orderId}' from header 'Sample.AzureTable.Transaction'");

            context.Extensions.Set(new TableEntityPartitionKey(orderId));
        }

        return next();
    }

}
#endregion