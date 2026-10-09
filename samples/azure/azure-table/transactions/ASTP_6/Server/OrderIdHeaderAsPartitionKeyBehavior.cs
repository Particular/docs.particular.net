using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Persistence.AzureTable;
using NServiceBus.Pipeline;
using NServiceBus.Sagas;

#region BehaviorUsingHeader
class OrderIdHeaderAsPartitionKeyBehavior(IProvidePartitionKeyFromSagaId partitionKeyFromSagaId, ILogger<OrderIdHeaderAsPartitionKeyBehavior> logger) : Behavior<IIncomingLogicalMessageContext>
{
    public override async Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        if (context.Headers.TryGetValue("Sample.AzureTable.Transaction.OrderId", out var orderId))
        {
            var correlationProperty = new SagaCorrelationProperty("OrderId", Guid.Parse(orderId));
            await partitionKeyFromSagaId.SetPartitionKey<OrderSagaData>(context, correlationProperty);

            logger.LogInformation("Found partition key '{PartitionKey}' from header 'Sample.AzureTable.Transaction.OrderId'", context.Extensions.Get<TableEntityPartitionKey>().PartitionKey);
        }

        await next();
    }

    public class Registration : RegisterStep
    {
        public Registration() :
            base(nameof(OrderIdHeaderAsPartitionKeyBehavior),
                typeof(OrderIdHeaderAsPartitionKeyBehavior),
                "Determines the PartitionKey from a header",
                provider => new OrderIdHeaderAsPartitionKeyBehavior(
                    provider.GetRequiredService<IProvidePartitionKeyFromSagaId>(),
                    provider.GetRequiredService<ILogger<OrderIdHeaderAsPartitionKeyBehavior>>()))
        {
            InsertBefore(nameof(OrderIdAsPartitionKeyBehavior));
            InsertBefore(nameof(LogicalOutboxBehavior));
        }
    }
}
#endregion
