using NServiceBus;

class Upgrade
{
    void CallbackReceiverMaxConcurrency(BusConfiguration busConfiguration)
    {
        #region 3to4rabbitmq-config-callbackreceiver-thread-count

        var transport = busConfiguration.UseTransport<RabbitMQTransport>();
        transport.CallbackReceiverMaxConcurrency(10);

        #endregion
    }
}
