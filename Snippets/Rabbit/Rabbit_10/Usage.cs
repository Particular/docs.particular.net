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
        transport.UseConventionalRoutingTopology(QueueType.Quorum);

        #endregion
    }

    void UseDirectRoutingTopology(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-usedirectroutingtopology

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseDirectRoutingTopology(QueueType.Quorum);

        #endregion
    }

    void UseDirectRoutingTopologyWithCustomConventions(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-usedirectroutingtopologywithcustomconventions

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseDirectRoutingTopology(
            QueueType.Quorum,
            routingKeyConvention: MyRoutingKeyConvention,
            exchangeNameConvention: () => "MyTopic");

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
        transport.SetClientCertificate(new X509Certificate2("/path/to/certificate"));

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
        transport.DisableDurableExchangesAndQueues();

        #endregion
    }

    void AddClusterNode(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-add-cluster-node

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.AddClusterNode("node2", useTls: false);

        #endregion

        #region rabbitmq-add-cluster-node-with-port

        transport.AddClusterNode("node2", 5675, useTls: true);

        #endregion
    }

    void DisableDelayedDelivery(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-disable-delayed-delivery

        var rabbitMqTransport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Classic),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq",
            enableDelayedDelivery: false
        );

        endpointConfiguration.UseTransport(rabbitMqTransport);

        #endregion
    }

    void SetManagementApiConfiguration(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-management-api-configuration

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.ManagementApiConfiguration(
            url: "{scheme}://{host}:{port}",
            userName: "{username}",
            password: "{password}"
        );

        #endregion
    }
}
