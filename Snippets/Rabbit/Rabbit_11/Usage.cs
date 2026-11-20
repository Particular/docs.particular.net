using System;
using System.Security.Cryptography.X509Certificates;

using NServiceBus;
using RabbitMQ.Client;

class Usage
{
    public Usage(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-basic

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    public void CustomConnectionString(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-connectionstring-in-code

        var connectionString = Environment.GetEnvironmentVariable("RabbitMQConnectionString");

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: connectionString);

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    public void CustomIdStrategy(EndpointConfiguration endpointConfiguration)
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

    public void UseConventionalRoutingTopology(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-useconventionalroutingtopology

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    public void UseDirectRoutingTopology(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-usedirectroutingtopology

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Direct(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    public void UseDirectRoutingTopologyWithCustomConventions(EndpointConfiguration endpointConfiguration)
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

    public void UseRoutingTopology5_0(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-useroutingtopologyDelegate

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Custom(new MyRoutingTopology(createDurableExchangesAndQueues: true)),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    public void UseCustomCircuitBreakerSettings(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-custom-breaker-settings-time-to-wait-before-triggering-code

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.TimeToWaitBeforeTriggeringCircuitBreaker = TimeSpan.FromMinutes(2);

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    public void PrefetchMultiplier(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-prefetch-multiplier

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.PrefetchCountCalculation = maximumConcurrency => maximumConcurrency * 4;

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    public void PrefetchCount(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-prefetch-count

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.PrefetchCountCalculation = _ => 100;

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    public void SetClientCertificateFile(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-client-certificate-file

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.ClientCertificate = X509CertificateLoader.LoadPkcs12FromFile("path", "password");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    public void SetClientCertificate(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-client-certificate

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.ClientCertificate = X509CertificateLoader.LoadCertificateFromFile("/path/to/certificate");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    public void DisableRemoteCertificateValidation(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-disable-remote-certificate-validation

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.ValidateRemoteCertificate = false;

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

#pragma warning disable CS0618 // Type or member is obsolete
    public void UseExternalAuthMechanism(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-external-auth-mechanism

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.UseExternalAuthMechanism = true;

        endpointConfiguration.UseTransport(transport);

        #endregion
    }
#pragma warning restore CS0618 // Type or member is obsolete

    public void SetExternalAuthMechanism(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-set-external-auth-mechanism

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.AuthMechanisms = [new ExternalMechanismFactory()];

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    public void CustomAuthMechanisms(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-custom-auth-mechanisms

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.AuthMechanisms = [new CustomMechanismFactory(), new PlainMechanismFactory()];

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    public class CustomMechanismFactory : IAuthMechanismFactory
    {
        public string Name
        {
            get { return "CUSTOM"; }
        }

        public IAuthMechanism GetInstance()
        {
            return null;
        }
    }

    public void ChangeRequestedHeartbeatForDebugging(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-debug-api

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.HeartbeatInterval = TimeSpan.FromMinutes(10);

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    public void ChangeHeartbeatInterval(EndpointConfiguration endpointConfiguration)
    {
        #region change-heartbeat-interval

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.HeartbeatInterval = TimeSpan.FromSeconds(30);

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    public void ChangeNetworkRecoveryInterval(EndpointConfiguration endpointConfiguration)
    {
        #region change-network-recovery-interval

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.NetworkRecoveryInterval = TimeSpan.FromSeconds(30);

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    public void DisableDurableExchangesAndQueues(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-disable-durable-exchanges

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum, useDurableEntities: false),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    public void AddClusterNode(EndpointConfiguration endpointConfiguration)
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

    public void DisableDelayedDelivery(EndpointConfiguration endpointConfiguration)
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

    public void SetManagementApiConfiguration(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-management-api-configuration

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.ManagementApiConfiguration = new ManagementApiConfiguration(
            url: "{scheme}://{host}:{port}",
            userName: "{username}",
            password: "{password}");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    public void DisableBrokerRequirementChecks(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-disable-broker-requirement-checks

        var transport = new RabbitMQTransport(
            routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
            connectionString: "host=localhost;username=rabbitmq;password=rabbitmq");

        transport.DisabledBrokerRequirementChecks = BrokerRequirementChecks.Version310OrNewer | BrokerRequirementChecks.StreamsEnabled;

        endpointConfiguration.UseTransport(transport);

        #endregion
    }
}
