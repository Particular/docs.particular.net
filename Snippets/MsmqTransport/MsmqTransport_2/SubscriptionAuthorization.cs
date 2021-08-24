namespace Core6.Transports.QueueCreation
{
    using System;
    using NServiceBus;

    public class SubscriptionAuthorization
    {
        public SubscriptionAuthorization(EndpointConfiguration endpointConfiguration)
        {
            #region SubscriptionAuthorizer

            var routing = endpointConfiguration.UseTransport(new MsmqTransport());

            routing.SubscriptionAuthorizer(context =>
            {
                var headers = context.MessageHeaders;

                var subscriptionMessageType = headers[Headers.SubscriptionMessageType];

                // messageIntent will be either MessageIntent.Unsubscribe or MessageIntent.Subscribe
                var messageIntent = (MessageIntent)Enum.Parse(typeof(MessageIntent), headers[Headers.MessageIntent], true);

                var endpointName = headers[Headers.SubscriberEndpoint].ToLowerInvariant();

                // true to allow, false to disallow
                return true;
            });

            #endregion
        }
    }
}
