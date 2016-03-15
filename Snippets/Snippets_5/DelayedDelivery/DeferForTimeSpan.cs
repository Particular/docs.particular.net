namespace Snippets5.DelayedDelivery
{
    using System;
    using NServiceBus;
    using NServiceBus.Persistence;

    class DeferForTimeSpan
    {
        public void SendDelayedMessage()
        {
            BusConfiguration busConfiguration = null;
            IBus bus = null;
            #region configure-persistence-timeout
            busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Timeouts>();
            #endregion
            #region delayed-delivery-timespan
            bus.Defer(TimeSpan.FromMinutes(30), new MessageToBeSentLater());
            #endregion
        }

        class MessageToBeSentLater
        {
        }
    }
}
