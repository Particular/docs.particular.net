namespace Snippets6.DelayedDelivery
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Persistence;

    class DeferForTimeSpan
    {
        async Task SendDelayedMessage(EndpointConfiguration endpointConfiguration, IEndpointInstance endpoint, IMessageHandlerContext handlerContext)
        {
            #region configure-persistence-timeout

            endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Timeouts>();

            #endregion

            #region delayed-delivery-timespan

            SendOptions options = new SendOptions();

            options.DelayDeliveryWith(TimeSpan.FromMinutes(30));

            await handlerContext.Send(new MessageToBeSentLater(), options);
            // OR
            await endpoint.Send(new MessageToBeSentLater(), options);

            #endregion
        }

        class MessageToBeSentLater
        {
        }
    }
}