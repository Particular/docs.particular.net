using System;
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

    void UseConventionalRoutingTopology(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-useconventionalroutingtopology

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseConventionalRoutingTopology();

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
            exchangeNameConvention: () => "MyTopic");

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

    void UseRoutingTopology5_0(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-useroutingtopologyDelegate

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseCustomRoutingTopology(
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

    void SetClientCertificateFile(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-client-certificate-file

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.SetClientCertificate("path", "password");

        #endregion
    }

    void SetClientCertificate(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-client-certificate

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.SetClientCertificate(new X509Certificate2());

        #endregion
    }

    void EnableTimeoutManager(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-delay-enable-timeout-manager

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        var delayedDelivery = transport.DelayedDelivery();
        delayedDelivery.EnableTimeoutManager();

        #endregion
    }

    void DisableRemoteCertificateValidation(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-disable-remote-certificate-validation

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.DisableRemoteCertificateValidation();

        #endregion
    }

    void UseExternalAuthMechanism(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-external-auth-mechanism

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseExternalAuthMechanism();

        #endregion
    }

    void ChangeRequestedHeartbeat(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-connectionstring-debug

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.ConnectionString("host=broker1;RequestedHeartbeat=600");

        #endregion
    }

    void ChangeRequestedHeartbeatForDebugging(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-debug-api

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.SetHeartbeatInterval(TimeSpan.FromMinutes(10));

        #endregion
    }

    void ChangeHeartbeatInterval(EndpointConfiguration endpointConfiguration)
    {
        #region change-heartbeat-interval

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.SetHeartbeatInterval(TimeSpan.FromSeconds(30));

        #endregion
    }

    void ChangeNetworkRecoveryInterval(EndpointConfiguration endpointConfiguration)
    {
        #region change-network-recovery-interval

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.SetNetworkRecoveryInterval(TimeSpan.FromSeconds(30));

        #endregion
    }


    void DisableDurableExchangesAndQueues(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-disable-durable-exchanges

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseDurableExchangesAndQueues(false);

        #endregion
    }

    void DisableDurableMessagesAndEnableDurableExchangesAndQueues(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-disable-durable-messages

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        endpointConfiguration.DisableDurableMessages();
        transport.UseDurableExchangesAndQueues(true);

        #endregion
    }
}
