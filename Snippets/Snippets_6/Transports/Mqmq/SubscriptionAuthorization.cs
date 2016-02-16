namespace Snippets6.Transports.QueueCreation
{
    using System;
    using NServiceBus;

    public class SubscriptionAuthorization 
    {
        public SubscriptionAuthorization()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            #region SubscriptionAuthorizer
            endpointConfiguration.UseTransport<MsmqTransport>()
                .SubscriptionAuthorizer(context =>
                {
                    string subscriptionMessageType = context.MessageHeaders[Headers.SubscriptionMessageType];
                    string messageIntent = context.MessageHeaders[Headers.MessageIntent];
                    MessageIntentEnum messageIntentEnum = (MessageIntentEnum)Enum.Parse(typeof(MessageIntentEnum), messageIntent, true);
                    //messageIntentEnum will be either MessageIntentEnum.Unsubscribe or MessageIntentEnum.Subscribe
                    string endpointName = context.MessageHeaders[Headers.SubscriberEndpoint]
                        .ToLowerInvariant();
                    // true to allow false to decline
                    return true;
                });
            #endregion
        }
    }
}