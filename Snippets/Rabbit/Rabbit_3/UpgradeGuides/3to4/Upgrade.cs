using NServiceBus;

class Upgrade
{
    void PrefetchCountReplacement(BusConfiguration busConfiguration)
    {
        #region 3to4rabbitmq-config-prefetch-count-replacement

        var transport = busConfiguration.UseTransport<RabbitMQTransport>();
        transport.ConnectionString("host=broker1;PrefetchCount=10");

        #endregion
    }

    void CallbackReceiverMaxConcurrency(BusConfiguration busConfiguration)
    {
        #region 3to4rabbitmq-config-callbackreceiver-thread-count

        var transport = busConfiguration.UseTransport<RabbitMQTransport>();
        transport.CallbackReceiverMaxConcurrency(10);

        #endregion
    }


}
