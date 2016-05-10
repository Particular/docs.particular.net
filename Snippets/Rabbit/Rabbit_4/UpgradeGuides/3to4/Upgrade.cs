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

    void CallbackReceiverMaxConcurrency(EndpointConfiguration endpointConfiguration)
    {
        #region 3to4rabbitmq-config-callbackreceiver-thread-count
        endpointConfiguration.LimitMessageProcessingConcurrencyTo(10);

        #endregion
    }

    void UseDirectRoutingTopology(EndpointConfiguration endpointConfiguration)
    {
        #region 3to4rabbitmq-config-usedirectroutingtopology

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseDirectRoutingTopology(MyRoutingKeyConvention, (address, eventType) => "MyTopic");

        #endregion
    }

    void ChangeTransactionMode(EndpointConfiguration endpointConfiguration)
    {
        #region 3to4rabbitmq-config-transactions

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.Transactions(TransportTransactionMode.None);

        #endregion
    }

    string MyRoutingKeyConvention(Type type)
    {
        throw new NotImplementedException();
    }
}
