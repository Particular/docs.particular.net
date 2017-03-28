﻿using System;
using System.Security.Cryptography.X509Certificates;
using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-basic

        endpointConfiguration.UseTransport<RabbitMQTransport>();

        #endregion
    }

    void CustomConnectionString(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-connectionstring-in-code

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.ConnectionString("My custom connection string");

        #endregion
    }

    void CustomConnectionStringName(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-connectionstringname

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.ConnectionStringName("MyConnectionStringName");

        #endregion
    }

    void CustomIdStrategy(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-custom-id-strategy

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.CustomMessageIdStrategy(
            customIdStrategy: deliveryArgs =>
            {
                var headers = deliveryArgs.BasicProperties.Headers;
                return headers["MyCustomId"].ToString();
            });

        #endregion
    }

    void UseDirectRoutingTopology(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-usedirectroutingtopology

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseDirectRoutingTopology();

        #endregion
    }

    void UseDirectRoutingTopologyWithCustomConventions(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-usedirectroutingtopologywithcustomconventions

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseDirectRoutingTopology(
            routingKeyConvention: MyRoutingKeyConvention,
            exchangeNameConvention: (address, eventType) => "MyTopic");

        #endregion
    }

    void DisablePublisherConfirms(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-disablepublisherconfirms

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UsePublisherConfirms(false);

        #endregion
    }

    string MyRoutingKeyConvention(Type type)
    {
        throw new NotImplementedException();
    }

#pragma warning disable CS0618
    void UseRoutingTopology4_0(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-useroutingtopology [4.0,4.1)

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseRoutingTopology<MyRoutingTopology>();

        #endregion
    }
#pragma warning restore CS0618

    void UseRoutingTopology4_1(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-useroutingtopologyDelegate 4.1

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseRoutingTopology(
            topologyFactory: createDurableExchangesAndQueues =>
            {
                return new MyRoutingTopology(createDurableExchangesAndQueues);
            });

        #endregion
    }

    void UseCustomCircuitBreakerSettings(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-custom-breaker-settings-time-to-wait-before-triggering-code

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan.FromMinutes(2));

        #endregion
    }

    void PrefetchMultiplier(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-prefetch-multiplier

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.PrefetchMultiplier(4);

        #endregion
    }

    void PrefetchCount(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-prefetch-count

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.PrefetchCount(100);

        #endregion
    }

    void SetClientCertificates(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-client-certificates

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.SetClientCertificates(new X509CertificateCollection());

        #endregion
    }

    void DisableTimeoutManager(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-delay-disable-timeout-manager 4.3

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        var delayedDelivery = transport.DelayedDelivery();
        delayedDelivery.DisableTimeoutManager();

        #endregion
    }

    void AllEndpointsSupportDelayedDelivery(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-delay-all-endpoints-support-delayed-delivery 4.3

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        var delayedDelivery = transport.DelayedDelivery();
        delayedDelivery.AllEndpointsSupportDelayedDelivery();

        #endregion
    }
}
