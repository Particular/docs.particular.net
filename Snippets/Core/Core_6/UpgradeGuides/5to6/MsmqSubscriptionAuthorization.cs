namespace Core6.UpgradeGuides._5to6
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
                    var headers = context.MessageHeaders;
                    var subscriptionMessageType = headers[Headers.SubscriptionMessageType];
                    var messageIntent = headers[Headers.MessageIntent];
                    var messageIntentEnum = (MessageIntentEnum)Enum.Parse(typeof(MessageIntentEnum), messageIntent, true);
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