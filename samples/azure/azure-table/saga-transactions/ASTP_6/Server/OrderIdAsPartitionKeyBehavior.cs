using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Persistence.AzureTable;
using NServiceBus.Pipeline;
using NServiceBus.Sagas;

#region BehaviorUsingIProvidePartitionKeyFromSagaId

class OrderIdAsPartitionKeyBehavior : Behavior<IIncomingLogicalMessageContext>
{
    public OrderIdAsPartitionKeyBehavior(IProvidePartitionKeyFromSagaId partitionKeyFromSagaId,
        ILogger<OrderIdAsPartitionKeyBehavior> logger)
    {
        partitionKeyFromSagaId1 = partitionKeyFromSagaId;
        this.logger = logger;
    }

    public override async Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        var correlationProperty = SagaCorrelationProperty.None;
        if (context.Message.Instance is IProvideOrderId provideOrderId)
        {
            var partitionKeyValue = provideOrderId.OrderId;
            correlationProperty = new SagaCorrelationProperty("OrderId", partitionKeyValue);
        }

        await partitionKeyFromSagaId1.SetPartitionKey<OrderSagaData>(context, correlationProperty);

        if (context.Headers.TryGetValue(Headers.SagaId, out var sagaIdHeader))
        {
            logger.LogInformation("Saga Id Header: {SagaIdHeader}", sagaIdHeader);
        }

        if (context.Extensions.TryGet<TableInformation>(out var tableInformation))
        {

            logger.LogInformation("Table Information: {TableName}", tableInformation.TableName);
        }


        logger.LogInformation("Found partition key '{PartitionKey}' from '{TypeName}'", context.Extensions.Get<TableEntityPartitionKey>().PartitionKey, nameof(IProvideOrderId));

        await next();
    }

    public class Registration : RegisterStep
    {
        public Registration() :
            base(nameof(OrderIdAsPartitionKeyBehavior),
                typeof(OrderIdAsPartitionKeyBehavior),
                "Determines the PartitionKey from the logical message",
                provider => new OrderIdAsPartitionKeyBehavior(
                    provider.GetRequiredService<IProvidePartitionKeyFromSagaId>(),
                    provider.GetRequiredService<ILogger<OrderIdAsPartitionKeyBehavior>>()
                    ))
        {
            InsertBefore(nameof(LogicalOutboxBehavior));
        }
    }

    IProvidePartitionKeyFromSagaId partitionKeyFromSagaId1;
    private readonly ILogger<OrderIdAsPartitionKeyBehavior> logger;
}
#endregion