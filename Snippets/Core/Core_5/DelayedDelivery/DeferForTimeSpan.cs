namespace Core5.DelayedDelivery
{
    using System;
    using NServiceBus;
    using NServiceBus.Persistence;

    class DeferForTimeSpan
    {
        void SendDelayedMessage(BusConfiguration busConfiguration, IBus bus)
        {
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
