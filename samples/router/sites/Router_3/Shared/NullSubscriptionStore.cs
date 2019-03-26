using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus.Extensibility;
using NServiceBus.Unicast.Subscriptions;
using NServiceBus.Unicast.Subscriptions.MessageDrivenSubscriptions;

public class NullSubscriptionStore : ISubscriptionStorage
{
    public Task Subscribe(Subscriber subscriber, MessageType messageType, ContextBag context)
    {
        return Task.CompletedTask;
    }

    public Task Unsubscribe(Subscriber subscriber, MessageType messageType, ContextBag context)
    {
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Subscriber>> GetSubscriberAddressesForMessage(IEnumerable<MessageType> messageTypes, ContextBag context)
    {
        return Task.FromResult(Enumerable.Empty<Subscriber>());
    }
}