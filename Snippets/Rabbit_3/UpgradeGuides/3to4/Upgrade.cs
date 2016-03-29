namespace Rabbit_3.UpgradeGuides._3to4
{
    using NServiceBus;

    class Upgrade
    {
        void CallbackReceiverMaxConcurrency(BusConfiguration busConfiguration)
        {
            #region 3to4rabbitmq-config-callbackreceiver-thread-count

            busConfiguration.UseTransport<RabbitMQTransport>()
                .CallbackReceiverMaxConcurrency(10);

            #endregion
        }
    }
}