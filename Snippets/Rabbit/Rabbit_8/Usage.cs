using System;
using System.Security.Cryptography.X509Certificates;
using NServiceBus;
using NServiceBus.Transport.RabbitMQ;

class Usage
{
    void Basic(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-basic

        endpointConfiguration.UseTransport(
            new RabbitMQTransport(Topology.Conventional, "host=localhost"));

        #endregion
    }

    void CustomConnectionString(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-connectionstring-in-code

        endpointConfiguration.UseTransport(
            new RabbitMQTransport(Topology.Conventional, "My custom connection string"));

        #endregion
    }

    void CustomIdStrategy(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-custom-id-strategy

        var transport = new RabbitMQTransport(Topology.Conventional, "host=localhost")
        {
            MessageIdStrategy = deliveryArgs =>
            {
                var headers = deliveryArgs.BasicProperties.Headers;
                return headers["MyCustomId"].ToString();
            }
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void UseConventionalRoutingTopology(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-useconventionalroutingtopology

        var transport = new RabbitMQTransport(Topology.Conventional, "host=localhost");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void UseDirectRoutingTopology(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-usedirectroutingtopology

        var transport = new RabbitMQTransport(Topology.Direct, "host=localhost");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void UseDirectRoutingTopologyWithCustomConventions(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-usedirectroutingtopologywithcustomconventions

        var topology = new DirectRoutingTopology(
            useDurableEntities: true,
            exchangeNameConvention: () => "MyTopic",
            routingKeyConvention: MyRoutingKeyConvention);

        var transport = new RabbitMQTransport(topology, "host=localhost");

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

        var topology = new MyRoutingTopology(createDurableExchangesAndQueues: true);

        var transport = new RabbitMQTransport(topology, "host=localhost");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void UseCustomCircuitBreakerSettings(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-custom-breaker-settings-time-to-wait-before-triggering-code

        var transport = new RabbitMQTransport(Topology.Conventional, "host=localhost")
        {
            TimeToWaitBeforeTriggeringCircuitBreaker = TimeSpan.FromMinutes(2)
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void PrefetchMultiplier(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-prefetch-multiplier

        var transport = new RabbitMQTransport(Topology.Conventional, "host=localhost")
        {
            PrefetchCountCalculation = concurrency => concurrency * 4
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void PrefetchCount(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-prefetch-count

        var transport = new RabbitMQTransport(Topology.Conventional, "host=localhost")
        {
            PrefetchCountCalculation = _ => 100
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void SetClientCertificate(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-client-certificate

        var transport = new RabbitMQTransport(Topology.Conventional, "host=localhost")
        {
            ClientCertificate = new X509Certificate2()
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void DisableRemoteCertificateValidation(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-disable-remote-certificate-validation

        var transport = new RabbitMQTransport(Topology.Conventional, "host=localhost")
        {
            ValidateRemoteCertificate = false
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void UseExternalAuthMechanism(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-external-auth-mechanism

        var transport = new RabbitMQTransport(Topology.Conventional, "host=localhost")
        {
            UseExternalAuthMechanism = true
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void ChangeRequestedHeartbeat(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-connectionstring-debug

        var transport = new RabbitMQTransport(Topology.Conventional, "host=broker1;RequestedHeartbeat=600")
        {
            ClientCertificate = new X509Certificate2()
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void ChangeRequestedHeartbeatForDebugging(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-debug-api

        var transport = new RabbitMQTransport(Topology.Conventional, "host=localhost")
        {
            HeartbeatInterval = TimeSpan.FromMinutes(10)
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void ChangeHeartbeatInterval(EndpointConfiguration endpointConfiguration)
    {
        #region change-heartbeat-interval

        var transport = new RabbitMQTransport(Topology.Conventional, "host=localhost")
        {
            HeartbeatInterval = TimeSpan.FromSeconds(30)
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void ChangeNetworkRecoveryInterval(EndpointConfiguration endpointConfiguration)
    {
        #region change-network-recovery-interval

        var transport = new RabbitMQTransport(Topology.Conventional, "host=localhost")
        {
            NetworkRecoveryInterval = TimeSpan.FromSeconds(30)
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }


    void DisableDurableExchangesAndQueues(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-disable-durable-exchanges

        var topology = new ConventionalRoutingTopology(
            useDurableEntities:  false);

        var transport = new RabbitMQTransport(topology, "host=localhost");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }
}
