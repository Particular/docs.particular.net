using NServiceBus.Pipeline;

namespace AsyncAPI.Feature;

class ReplaceOutgoingEnclosedMessageTypeHeaderBehavior(Dictionary<Type, Type> publishedEventCache) : IBehavior<IOutgoingPhysicalMessageContext,
    IOutgoingPhysicalMessageContext>
{
    public Task Invoke(IOutgoingPhysicalMessageContext context, Func<IOutgoingPhysicalMessageContext, Task> next)
    {
        var logicalMessage = context.Extensions.Get<OutgoingLogicalMessage>();
        if (publishedEventCache.TryGetValue(logicalMessage.MessageType, out var publishedEvent))
        {
            // very blunt and might break with certain transports
            context.Headers[Headers.EnclosedMessageTypes] = publishedEvent.FullName;
        }

        return next(context);
    }
}