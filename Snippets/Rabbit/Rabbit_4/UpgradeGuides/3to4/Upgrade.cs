using System;
using NServiceBus;

class Upgrade
{
    void UseCustomCircuitBreakerSettings(EndpointConfiguration endpointConfiguration)
    {
        #region 3to4rabbitmq-custom-breaker-settings-code

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan.FromMinutes(2));

        #endregion
    }

    void PrefetchCountReplacement(EndpointConfiguration endpointConfiguration)
    {
        #region 3to4rabbitmq-config-prefetch-count-replacement

        endpointConfiguration.LimitMessageProcessingConcurrencyTo(10);

        #endregion
    }
}
