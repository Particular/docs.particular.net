using System;
using NServiceBus;

partial class Upgrade
{
    void PrefetchMultiplier(EndpointConfiguration endpointConfiguration)
    {
        #region 3to4rabbitmq-config-prefetch-multiplier

        endpointConfiguration.LimitMessageProcessingConcurrencyTo(10);
        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        // Prefetch count set to 10 * 4 = 40
        transport.PrefetchMultiplier(4);

        #endregion
    }

    void PrefetchCount(EndpointConfiguration endpointConfiguration)
    {
        #region 3to4rabbitmq-config-prefetch-count

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.PrefetchCount(100);

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

    void UsePublisherConfirms(EndpointConfiguration endpointConfiguration)
    {
        #region 3to4rabbitmq-use-publisher-confirms

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UsePublisherConfirms(true);

        #endregion
    }
}
