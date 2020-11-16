using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.AzureTable;
using NServiceBus.Pipeline;
using NServiceBus.Sagas;

#region BehaviorUsingIProvidePartitionKeyFromSagaId

class OrderIdAsPartitionKeyBehavior : Behavior<IIncomingLogicalMessageContext>
{
    public OrderIdAsPartitionKeyBehavior(IProvidePartitionKeyFromSagaId partitionKeyFromSagaId)
    {
        partitionKeyFromSagaId1 = partitionKeyFromSagaId;
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
            Log.Info($"Saga Id Header: {sagaIdHeader}");
        }

        if (context.Extensions.TryGet<TableInformation>(out var tableInformation))
        {
            Log.Info($"Table Information: {tableInformation.TableName}");
        }

        Log.Info($"Found partition key '{context.Extensions.Get<TableEntityPartitionKey>().PartitionKey}' from '{nameof(IProvideOrderId)}'");

        await next().ConfigureAwait(false);
    }

    public class Registration : RegisterStep
    {
        public Registration() :
            base(nameof(OrderIdAsPartitionKeyBehavior),
                typeof(OrderIdAsPartitionKeyBehavior),
                "Determines the PartitionKey from the logical message",
                b => new OrderIdAsPartitionKeyBehavior(b.Build<IProvidePartitionKeyFromSagaId>()))
        {
            InsertBefore(nameof(LogicalOutboxBehavior));
        }
    }

    IProvidePartitionKeyFromSagaId partitionKeyFromSagaId1;
    static readonly ILog Log = LogManager.GetLogger<OrderIdAsPartitionKeyBehavior>();
}
#endregion