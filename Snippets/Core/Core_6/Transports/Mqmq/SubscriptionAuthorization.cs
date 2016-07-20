namespace Core6.Transports.QueueCreation
{
    using System;
    using NServiceBus;

    public class SubscriptionAuthorization
    {
        public SubscriptionAuthorization(EndpointConfiguration endpointConfiguration)
        {
            #region SubscriptionAuthorizer

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            transport.SubscriptionAuthorizer(context =>
            {
                var headers = context.MessageHeaders;
                var subscriptionMessageType = headers[Headers.SubscriptionMessageType];
                var messageIntent = headers[Headers.MessageIntent];
                var messageIntentEnum = (MessageIntentEnum) Enum.Parse(typeof(MessageIntentEnum), messageIntent, true);
                // messageIntentEnum will be either MessageIntentEnum.Unsubscribe or MessageIntentEnum.Subscribe
                var endpointName = headers[Headers.SubscriberEndpoint]
                    .ToLowerInvariant();
                // true to allow false to decline
                return true;
            });

            #endregion
        }
    }
}