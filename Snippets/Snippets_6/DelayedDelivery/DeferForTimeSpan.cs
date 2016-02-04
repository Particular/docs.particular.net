namespace Snippets6.DelayedDelivery
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;

    class DeferForTimeSpan
    {
        public async Task SendDelayedMessage()
        {
            IBusSession busSession = null;
            IMessageHandlerContext handlerContext = null;

            #region delayed-delivery-timespan
            SendOptions options = new SendOptions();
            
            options.DelayDeliveryWith(TimeSpan.FromMinutes(30));

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
