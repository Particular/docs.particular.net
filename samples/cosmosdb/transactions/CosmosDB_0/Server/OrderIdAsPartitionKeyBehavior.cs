using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using NServiceBus.Logging;
using NServiceBus.Persistence.CosmosDB;
using NServiceBus.Pipeline;

#region BehaviorUsingIProvideOrderId

class OrderIdAsPartitionKeyBehavior : Behavior<IIncomingLogicalMessageContext>
{
    public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        if (context.Message.Instance is IProvideOrderId provideOrderId)
        {
            var partitionKeyValue = provideOrderId.OrderId.ToString();

            Log.Info($"Found partition key '{partitionKeyValue}' from '{nameof(IProvideOrderId)}'");

            context.Extensions.Set(new PartitionKey(partitionKeyValue));
        }

        return next();
    }

    public class Registration : RegisterStep
    {
        public Registration() :
            base(nameof(OrderIdAsPartitionKeyBehavior),
                typeof(OrderIdAsPartitionKeyBehavior),
                "Determines the PartitionKey from the logical message",
                b => new OrderIdAsPartitionKeyBehavior())
        {
            InsertBefore(nameof(LogicalOutboxBehavior));
        }
    }

    static readonly ILog Log = LogManager.GetLogger<OrderIdAsPartitionKeyBehavior>();
}
#endregion