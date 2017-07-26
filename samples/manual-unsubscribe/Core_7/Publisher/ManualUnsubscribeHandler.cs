using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Unicast.Subscriptions;
using NServiceBus.Unicast.Subscriptions.MessageDrivenSubscriptions;
using System.Threading.Tasks;

#region unsubscribe-handling

class ManualUnsubscribeHandler :
    IHandleMessages<ManualUnsubscribe>
{
    ISubscriptionStorage subscriptionStorage;

    public ManualUnsubscribeHandler(ISubscriptionStorage subscriptionStorage)
    {
        this.subscriptionStorage = subscriptionStorage;
    }

    public async Task Handle(ManualUnsubscribe message, IMessageHandlerContext context)
    {
        var emptyContext = new ContextBag();
        var type = Type.GetType(message.MessageTypeName, true);
        var messageType = new MessageType(type);
        var addressesForEndpoint = await GetAddressesForEndpoint(message.SubscriberEndpoint, messageType, emptyContext)
            .ConfigureAwait(false);
        await UnsubscribeFromEndpoint(addressesForEndpoint, messageType, emptyContext)
            .ConfigureAwait(false);
    }
    #endregion

    #region GetAddressesForEndpoint

    async Task<IEnumerable<Subscriber>> GetAddressesForEndpoint(string endpoint, MessageType messageType, ContextBag emptyContext)
    {
        var messageTypes = new List<MessageType>
        {
            messageType
        };
        var addressesForMessage = await subscriptionStorage.GetSubscriberAddressesForMessage(messageTypes, emptyContext)
            .ConfigureAwait(false);
        return addressesForMessage
            .Where(subscriber =>
            {
                return string.Equals(subscriber.Endpoint, endpoint, StringComparison.OrdinalIgnoreCase);
            });
    }

    #endregion
    #region UnsubscribeFromEndpoint
    Task UnsubscribeFromEndpoint(IEnumerable<Subscriber> addressesForEndpoint, MessageType messageType, ContextBag emptyContext)
    {
        var tasks = addressesForEndpoint
            .Select(address => subscriptionStorage.Unsubscribe(
                subscriber: address,
                messageType: messageType,
                context: emptyContext
            ));
        return Task.WhenAll(tasks);
    }
    #endregion
}
