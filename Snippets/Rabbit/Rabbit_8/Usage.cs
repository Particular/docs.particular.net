using System;
using System.Security.Cryptography.X509Certificates;
using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-basic

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void CustomConnectionString(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-connectionstring-in-code

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "My custom connection string");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void CustomIdStrategy(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-custom-id-strategy

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.MessageIdStrategy = deliveryArgs =>
        {
            var headers = deliveryArgs.BasicProperties.Headers;
            return headers["MyCustomId"].ToString();
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void UseConventionalRoutingTopology(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-useconventionalroutingtopology

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void UseDirectRoutingTopology(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-usedirectroutingtopology

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Direct(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void UseDirectRoutingTopologyWithCustomConventions(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-usedirectroutingtopologywithcustomconventions

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Direct(
                QueueType.Quorum,
                routingKeyConvention: MyRoutingKeyConvention,
                exchangeNameConvention: () => "MyTopic"),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    string MyRoutingKeyConvention(Type type)
    {
        throw new NotImplementedException();
    }

    void UseRoutingTopology5_0(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-useroutingtopologyDelegate

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Custom(new MyRoutingTopology(createDurableExchangesAndQueues: true)),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void UseCustomCircuitBreakerSettings(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-custom-breaker-settings-time-to-wait-before-triggering-code

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.TimeToWaitBeforeTriggeringCircuitBreaker = TimeSpan.FromMinutes(2);

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void PrefetchMultiplier(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-prefetch-multiplier

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.PrefetchCountCalculation = maximumConcurrency => maximumConcurrency * 4;

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void PrefetchCount(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-prefetch-count

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.PrefetchCountCalculation = _ => 100;

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void SetClientCertificateFile(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-client-certificate-file

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.ClientCertificate = new X509Certificate2("path", "password");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void SetClientCertificate(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-client-certificate

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.ClientCertificate = new X509Certificate2();

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void DisableRemoteCertificateValidation(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-disable-remote-certificate-validation

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.ValidateRemoteCertificate = false;

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void UseExternalAuthMechanism(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-external-auth-mechanism

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.UseExternalAuthMechanism = true;

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void ChangeRequestedHeartbeatForDebugging(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-debug-api

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.HeartbeatInterval = TimeSpan.FromMinutes(10);

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void ChangeHeartbeatInterval(EndpointConfiguration endpointConfiguration)
    {
        #region change-heartbeat-interval

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.HeartbeatInterval = TimeSpan.FromSeconds(30);

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void ChangeNetworkRecoveryInterval(EndpointConfiguration endpointConfiguration)
    {
        #region change-network-recovery-interval

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.NetworkRecoveryInterval = TimeSpan.FromSeconds(30);

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void DisableDurableExchangesAndQueues(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-disable-durable-exchanges

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum, useDurableEntities: false),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void AddClusterNode(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-add-cluster-node

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        endpointConfiguration.UseTransport(transport);

        transport.AddClusterNode("node2", useTls: false);

        #endregion

        #region rabbitmq-add-cluster-node-with-port

        transport.AddClusterNode("node2", 5675, useTls: true);

        #endregion
    }
}
