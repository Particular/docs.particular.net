using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using NServiceBus.Persistence.CosmosDB;
using NServiceBus.Pipeline;

#region ITransportReceiveContextBehavior
class PartitionKeyTransportReceiveContextBehavior
    : Behavior<ITransportReceiveContext>
{
    public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
    {
        context.Extensions.Set(new PartitionKey("PartitionKeyValue"));

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
        context.Extensions.Set(new PartitionKey("PartitionKeyValue"));

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
