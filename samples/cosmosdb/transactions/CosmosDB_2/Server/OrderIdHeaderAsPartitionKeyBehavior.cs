using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using NServiceBus.Logging;
using NServiceBus.Pipeline;

#region BehaviorUsingHeader
class OrderIdHeaderAsPartitionKeyBehavior : Behavior<ITransportReceiveContext>
{
    public override Task Invoke(ITransportReceiveContext context, Func<Task> next)
    {
        if (context.Message.Headers.TryGetValue("Sample.CosmosDB.Transaction.OrderId", out var orderId))
        {
            Log.Info($"Found partition key '{orderId}' from header 'Sample.CosmosDB.Transaction'");

            context.Extensions.Set(new PartitionKey(orderId));
        }

        return next();
    }

    static readonly ILog Log = LogManager.GetLogger<OrderIdAsPartitionKeyBehavior>();
}
#endregion