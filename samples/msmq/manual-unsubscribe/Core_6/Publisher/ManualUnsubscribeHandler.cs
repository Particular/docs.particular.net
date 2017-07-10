using Messages;
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

    public Task Handle(ManualUnsubscribe message, IMessageHandlerContext context)
    {
        return subscriptionStorage.Unsubscribe(
            subscriber: new Subscriber(message.SubscriberTransportAddress, message.SubscriberEndpoint),
            messageType: new MessageType(message.MessageTypeName, message.MessageVersion),
            context: new ContextBag()
        );
    }
}
#endregion