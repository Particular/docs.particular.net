using System;
using NServiceBus;

class Upgrade
{
    void PrefetchCountReplacement(EndpointConfiguration endpointConfiguration)
    {
        #region 3to4rabbitmq-config-prefetch-count-replacement

        endpointConfiguration.LimitMessageProcessingConcurrencyTo(10);

        #endregion
    }

    void CallbackReceiverMaxConcurrency(EndpointConfiguration endpointConfiguration)
    {
        #region 3to4rabbitmq-config-callbackreceiver-thread-count

        endpointConfiguration.LimitMessageProcessingConcurrencyTo(10);

        #endregion
    }

    void UseCustomCircuitBreakerSettings(EndpointConfiguration endpointConfiguration)
    {
        #region 3to4rabbitmq-custom-breaker-settings-time-to-wait-before-triggering

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan.FromMinutes(2));

        #endregion
    }

    void UsePublisherConfirmsSettings(EndpointConfiguration endpointConfiguration)
    {
        #region 3to4rabbitmq-use-publisher-confirms

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UsePublisherConfirms(true);

        #endregion
    }


}
