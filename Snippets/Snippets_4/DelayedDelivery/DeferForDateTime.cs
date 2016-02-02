namespace Snippets4.DelayedDelivery
{
    using System;
    using NServiceBus;

    class DeferForDateTime
    {
        IBus bus;

        public DeferForDateTime(IBus bus)
        {
            this.bus = bus;
        }

        public void SendDelayedMessage()
        {
            #region delayed-delivery-datetime
            bus.Defer(new DateTime(2016, 12, 25), new MessageToBeSentLater());
            #endregion
        }

        class MessageToBeSentLater
        {
        }
    }
}