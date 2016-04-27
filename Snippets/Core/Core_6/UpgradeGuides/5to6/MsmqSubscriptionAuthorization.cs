namespace Core6.Transports.QueueCreation
{
    using System;
    using NServiceBus;

    class MsmqSubscriptionAuthorization
    {
        MsmqSubscriptionAuthorization(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6-MsmqSubscriptionAuthorizer

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            transport.SubscriptionAuthorizer(context =>
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