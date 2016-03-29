namespace Rabbit_4.UpgradeGuides._3to4
{
    using System;
    using NServiceBus;

    class Upgrade
    {
        void UseCustomCircuitBreakerSettings(EndpointConfiguration endpointConfiguration)
        {
            #region 3to4rabbitmq-custom-breaker-settings-code

            endpointConfiguration.UseTransport<RabbitMQTransport>()
                .TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan.FromMinutes(2));

            #endregion
        }

        void CallbackReceiverMaxConcurrency(EndpointConfiguration endpointConfiguration)
        {
            #region 3to4rabbitmq-config-callbackreceiver-thread-count
            endpointConfiguration.LimitMessageProcessingConcurrencyTo(10);

            #endregion
        }
    }
}