namespace Core7.DelayedDelivery
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;

    class DeferForTimeSpan
    {
        async Task SendDelayedMessage(EndpointConfiguration endpointConfiguration, IEndpointInstance endpoint, IMessageHandlerContext handlerContext)
        {
            #region configure-persistence-timeout

            endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Timeouts>();

            #endregion

            #region delayed-delivery-timespan

            var sendOptions = new SendOptions();

            sendOptions.DelayDeliveryWith(TimeSpan.FromMinutes(30));

            await handlerContext.Send(new MessageToBeSentLater(), sendOptions)
                .ConfigureAwait(false);
            // OR
            await endpoint.Send(new MessageToBeSentLater(), sendOptions)
                .ConfigureAwait(false);

            #endregion
        }

        class MessageToBeSentLater
        {
        }
    }
}