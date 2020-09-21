using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using NServiceBus.Persistence.CosmosDB;
using NServiceBus.Pipeline;

class OrderIdAsPartitionKeyBehavior : Behavior<IIncomingLogicalMessageContext>
{
    public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        if (context.Message.Instance is IProvideOrderId provideOrderId)
        {
            context.Extensions.Set(new PartitionKey(provideOrderId.OrderId.ToString()));
        }
        return next();
    }

    public class Registration : RegisterStep
    {
        public Registration() :
            base(stepId: nameof(OrderIdAsPartitionKeyBehavior),
                behavior: typeof(OrderIdAsPartitionKeyBehavior),
                description: "Determines the PartitionKey from the logical message",
                factoryMethod: b => new OrderIdAsPartitionKeyBehavior())
        {
            InsertBefore(nameof(LogicalOutboxBehavior));
        }
    }
}