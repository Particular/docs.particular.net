namespace Snippets5.DelayedDelivery
{
    using System;
    using NServiceBus;

    class DeferForTimeSpan
    {
        public void SendDelayedMessage()
        {
            IBus bus = null;
            #region delayed-delivery-timespan
            bus.Defer(TimeSpan.FromMinutes(30), new MessageToBeSentLater());
            #endregion
        }

        class MessageToBeSentLater
        {
        }
    }
}
