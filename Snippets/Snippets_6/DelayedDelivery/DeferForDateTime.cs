namespace Snippets6.DelayedDelivery
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;

    class DeferForDateTime
    {
        public async Task SendDelayedMessage()
        {
            IBusSession busSession = null;
            IMessageHandlerContext handlerContext = null;

            #region delayed-delivery-datetime
            SendOptions options = new SendOptions();
            options.DoNotDeliverBefore(new DateTime(2016, 12, 25));

            await handlerContext.Send(new MessageToBeSentLater(), options);
            // OR
            await busSession.Send(new MessageToBeSentLater(), options);
            #endregion
        }

        class MessageToBeSentLater
        {
        }
    }
}