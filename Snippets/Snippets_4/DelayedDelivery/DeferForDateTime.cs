namespace Snippets4.DelayedDelivery
{
    using System;
    using NServiceBus;

    class DeferForDateTime
    {
        public void SendDelayedMessage()
        {
            IBus bus = null;
            #region delayed-delivery-datetime
            bus.Defer(new DateTime(2016, 12, 25), new MessageToBeSentLater());
            #endregion
        }

        class MessageToBeSentLater
        {
        }
    }
}