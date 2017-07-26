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
        var messageTypes = new List<MessageType>
        {
            messageType
        };
        var addressesForMessage = await subscriptionStorage.GetSubscriberAddressesForMessage(messageTypes, emptyContext)
            .ConfigureAwait(false);
        var tasks = addressesForMessage
            .Where(subscriber =>
            {
                return string.Equals(subscriber.Endpoint, message.SubscriberEndpoint, StringComparison.OrdinalIgnoreCase);
            })
            .Select(address => subscriptionStorage.Unsubscribe(
                subscriber: address,
                messageType: messageType,
                context: emptyContext
            ));
        await Task.WhenAll(tasks)
            .ConfigureAwait(false);
    }
}

#endregion