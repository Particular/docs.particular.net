using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.AzureTable;
using NServiceBus.Pipeline;
using NServiceBus.Sagas;

#region ITransportReceiveContextBehavior
class PartitionKeyTransportReceiveContextBehavior
    : Behavior<ITransportReceiveContext>
{
    public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
    {
        context.Extensions.Set(new TableEntityPartitionKey("PartitionKeyValue"));

        await next().ConfigureAwait(false);
    }
}
#endregion

#region IIncomingLogicalMessageContextBehavior
class PartitionKeyIncomingLogicalMessageContextBehavior
    : Behavior<IIncomingLogicalMessageContext>
{
    public override async Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        context.Extensions.Set(new TableEntityPartitionKey("PartitionKeyValue"));

        await next().ConfigureAwait(false);
    }
}
#endregion

#region InsertBeforeLogicalOutbox
public class RegisterMyBehavior : RegisterStep
{
    public RegisterMyBehavior() :
        base(stepId: nameof(PartitionKeyIncomingLogicalMessageContextBehavior),
        behavior: typeof(PartitionKeyIncomingLogicalMessageContextBehavior),
        description: "Determines the PartitionKey from the logical message",
        factoryMethod: b => new PartitionKeyIncomingLogicalMessageContextBehavior())
    {
        InsertBeforeIfExists(nameof(LogicalOutboxBehavior));
    }
}
#endregion

#region CustomTableNameUsingITransportReceiveContextBehavior

class ContainerInfoTransportReceiveContextBehavior
    : Behavior<ITransportReceiveContext>
{
    public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
    {
        context.Extensions.Set(
            new TableInformation(
                tableName: "tableName"));

        await next().ConfigureAwait(false);
    }
}

#endregion

#region CustomTableNameUsingIIncomingLogicalMessageContextBehavior

class ContainerInfoLogicalReceiveContextBehavior
    : Behavior<IIncomingLogicalMessageContext>
{
    public override async Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        context.Extensions.Set(
            new TableInformation(
                tableName: "tableName"));

        await next().ConfigureAwait(false);
    }
}

#endregion

#region BehaviorUsingIProvidePartitionKeyFromSagaId

class OrderIdAsPartitionKeyBehavior : Behavior<IIncomingLogicalMessageContext>
{
    public OrderIdAsPartitionKeyBehavior(IProvidePartitionKeyFromSagaId partitionKeyFromSagaId) =>
        this.partitionKeyFromSagaId = partitionKeyFromSagaId;

    public override async Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        var correlationProperty = SagaCorrelationProperty.None;

        if (context.Message.Instance is IProvideOrderId provideOrderId)
        {
            Log.Debug($"Order ID: '{provideOrderId.OrderId}'");

            correlationProperty = new SagaCorrelationProperty("OrderId", provideOrderId.OrderId);
        }

        await partitionKeyFromSagaId.SetPartitionKey<OrderSagaData>(context, correlationProperty)
            .ConfigureAwait(false);

        Log.Debug($"Partition key: {context.Extensions.Get<TableEntityPartitionKey>().PartitionKey}");

        if (context.Headers.TryGetValue(Headers.SagaId, out var sagaId))
        {
            Log.Debug($"Saga ID: {sagaId}");
        }

        if (context.Extensions.TryGet<TableInformation>(out var tableInformation))
        {
            Log.Debug($"Table name: {tableInformation.TableName}");
        }

        await next().ConfigureAwait(false);
    }

    public class Registration : RegisterStep
    {
        public Registration() :
            base(nameof(OrderIdAsPartitionKeyBehavior),
                typeof(OrderIdAsPartitionKeyBehavior),
                "Determines the PartitionKey from the logical message",
                b => new OrderIdAsPartitionKeyBehavior(b.Build<IProvidePartitionKeyFromSagaId>())) =>
            InsertBefore(nameof(LogicalOutboxBehavior));
    }

    readonly IProvidePartitionKeyFromSagaId partitionKeyFromSagaId;
    static readonly ILog Log = LogManager.GetLogger<OrderIdAsPartitionKeyBehavior>();
}
#endregion

public interface IProvideOrderId
{
    Guid OrderId { get; }
}

public class OrderSagaData :
    ContainSagaData
{
    public Guid OrderId { get; set; }
    public string OrderDescription { get; set; }
}
