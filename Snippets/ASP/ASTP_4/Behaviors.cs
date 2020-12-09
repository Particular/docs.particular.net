using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
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
                provider => new OrderIdAsPartitionKeyBehavior(provider.GetRequiredService<IProvidePartitionKeyFromSagaId>()))
        {
            InsertBefore(nameof(LogicalOutboxBehavior));
        }
    }

    IProvidePartitionKeyFromSagaId partitionKeyFromSagaId1;
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
