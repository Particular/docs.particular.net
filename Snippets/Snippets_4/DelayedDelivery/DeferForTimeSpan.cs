namespace Snippets4.DelayedDelivery
{
    using System;
    using NServiceBus;

    class DeferForTimeSpan
    {
        IBus bus;

        public DeferForTimeSpan(IBus bus)
        {
            this.bus = bus;
        }

        public void SendDelayedMessage()
        {
            #region delayed-delivery-timespan
            bus.Defer(TimeSpan.FromMinutes(30), new MessageToBeSentLater());
            #endregion
        }

        class MessageToBeSentLater
        {
        }
    }
}
