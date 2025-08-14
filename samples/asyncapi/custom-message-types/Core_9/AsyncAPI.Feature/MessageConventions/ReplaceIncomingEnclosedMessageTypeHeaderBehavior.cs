using NServiceBus.Pipeline;

namespace AsyncAPI.Feature;

class ReplaceIncomingEnclosedMessageTypeHeaderBehavior(Dictionary<string, (Type SubscribedType, Type ActualType)> subscribedEventCache)
    : IBehavior<ITransportReceiveContext, ITransportReceiveContext>
{
    public Task Invoke(ITransportReceiveContext context, Func<ITransportReceiveContext, Task> next)
    {
        if (context.Message.Headers.TryGetValue(Headers.EnclosedMessageTypes, out var enclosedMessageTypes) && subscribedEventCache.TryGetValue(enclosedMessageTypes, out var subscribedEventType))
        {
            // very blunt and might break with certain transports
            context.Message.Headers[Headers.EnclosedMessageTypes] = subscribedEventType.ActualType.FullName;
        }
        return next(context);
    }
}