using System;
using System.Threading.Tasks;
using NServiceBus.ObjectBuilder;
using NServiceBus.Pipeline;

class DispatchLegacyBusMessgesBehavior : Behavior<IIncomingLogicalMessageContext>
{
    public DispatchLegacyBusMessgesBehavior()
    {
    }

    public override async Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        var legacyBus = context.Builder.Build<LegacyBus>();
        legacyBus.Initialize(context.MessageId, context.MessageHeaders);
        await next().ConfigureAwait(false); //Execute the handlers
        await legacyBus.DispatchMessages(context);
    }
}