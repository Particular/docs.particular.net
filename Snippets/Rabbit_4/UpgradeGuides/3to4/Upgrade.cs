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
    }
}