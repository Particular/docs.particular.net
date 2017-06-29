using Messages;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Unicast.Subscriptions;
using NServiceBus.Unicast.Subscriptions.MessageDrivenSubscriptions;
using System.Threading.Tasks;

namespace Publisher
{
    class ManualUnsubscribeHandler : IHandleMessages<ManualUnsubscribe>
    {
        ISubscriptionStorage subscriptionStorage;

        public ManualUnsubscribeHandler(ISubscriptionStorage subscriptionStorage)
        {
            this.subscriptionStorage = subscriptionStorage;
        }

        public async Task Handle(ManualUnsubscribe message, IMessageHandlerContext context)
        {
            await subscriptionStorage.Unsubscribe(
                    subscriber: new Subscriber(message.SubscriberTransportAddress, message.SubscriberEndpoint),
                    messageType: new MessageType(message.MessageTypeName, message.MessageVersion), 
                    context: new ContextBag()
                ).ConfigureAwait(false);
        }
    }
}
