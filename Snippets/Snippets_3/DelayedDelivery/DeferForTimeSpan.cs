namespace Snippets3.DelayedDelivery
{
    using System;
    using NServiceBus;

    class DeferForTimeSpan
    {
        public void SendDelayedMessage()
        {
            Configure configuration = null;
            IBus bus = null;
            #region configure-persistence-timeout
            configuration.UseNHibernateTimeoutPersister();
            #endregion
            #region configure-timeout-manager
            configuration.RunTimeoutManager();
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
