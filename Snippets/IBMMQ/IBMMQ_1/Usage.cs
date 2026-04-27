using System;
using NServiceBus;
using NServiceBus.Transport.IBMMQ;

class Usage
{
    public void BasicConfig()
    {
        #region ibmmq-config-basic

        var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

        var transport = new IBMMQTransport
        {
            Host = "mq-server.example.com", // MQ server hostname
            Port = 1414, // Listener port
            Channel = "DEV.APP.SVRCONN",  // Client connection channel
            QueueManagerName = "QM1", //  Queue Manager
            User = "app",// MQ username
            Password = "passw0rd"// MQ password
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    public void BasicConnection()
    {
        #region ibmmq-basic-connection

        var transport = new IBMMQTransport
        {
            Host = "mq-server.example.com",
            Port = 1414,
            Channel = "DEV.APP.SVRCONN",
            QueueManagerName = "QM1"
        };

        #endregion
    }

    public void Authentication()
    {
        #region ibmmq-authentication

        var transport = new IBMMQTransport
        {
            Host = "mq-server.example.com",
            QueueManagerName = "QM1",
            User = "app",
            Password = "passw0rd"
        };

        #endregion
    }

    public void ApplicationName()
    {
        #region ibmmq-application-name

        var transport = new IBMMQTransport
        {
            // ... connection settings ...
            ApplicationName = "OrderService"
        };

        #endregion
    }

    public void HighAvailability()
    {
        #region ibmmq-high-availability

        var transport = new IBMMQTransport
        {
            QueueManagerName = "QM1",
            Channel = "APP.SVRCONN",
            Connections = { "mqhost1(1414)", "mqhost2(1414)" }
        };

        #endregion
    }

    public void SslTls()
    {
        #region ibmmq-ssl-tls

        var transport = new IBMMQTransport
        {
            Host = "mq-server.example.com",
            QueueManagerName = "QM1",
            SslKeyRepository = "*SYSTEM",
            CipherSpec = "TLS_RSA_WITH_AES_256_CBC_SHA256"
        };

        #endregion
    }

    public void SslPeerName()
    {
        #region ibmmq-ssl-peer-name

        var transport = new IBMMQTransport
        {
            // ... connection settings ...
            SslKeyRepository = "*SYSTEM",
            CipherSpec = "TLS_RSA_WITH_AES_256_CBC_SHA256",
            SslPeerName = "CN=MQSERVER01,O=MyCompany,C=US"
        };

        #endregion
    }

    public void CustomTopicPrefix()
    {
        #region ibmmq-custom-topic-prefix

        var transport = new IBMMQTransport
        {
            // ... connection settings ...
            TopicNaming = new TopicNaming("PROD")
        };

        #endregion
    }

    public void CustomTopicNamingUsage()
    {
        #region ibmmq-custom-topic-naming-usage

        var transport = new IBMMQTransport
        {
            // ... connection settings ...
            TopicNaming = new ShortTopicNaming()
        };

        #endregion
    }

    public void ResourceSanitization()
    {
        #region ibmmq-resource-sanitization

        var transport = new IBMMQTransport
        {
            // ... connection settings ...
            ResourceNameSanitizer = name =>
            {
                var sanitized = name.Replace("-", ".").Replace("/", ".");
                return sanitized.Length > 48 ? sanitized[..48] : sanitized;
            }
        };

        #endregion
    }

    public void PollingInterval()
    {
        #region ibmmq-polling-interval

        var transport = new IBMMQTransport
        {
            // ... connection settings ...
            MessageWaitInterval = TimeSpan.FromMilliseconds(2000)
        };

        #endregion
    }

    public void CharacterSet()
    {
        #region ibmmq-character-set

        var transport = new IBMMQTransport
        {
            // ... connection settings ...
            CharacterSet = 1208 // UTF-8 (default)
        };

        #endregion
    }

    public void ExplicitSubscribeRoutes()
    {
        #region ibmmq-explicit-subscribe-routes

        var transport = new IBMMQTransport
        {
            // ... connection settings ...
        };

        // Subscribe to OrderPlaced and all known concrete types
        transport.Topology.SubscribeTo<IOrderEvent, OrderPlaced>();
        transport.Topology.SubscribeTo<IOrderEvent, ExpressOrderPlaced>();

        #endregion
    }

    public void ExplicitSubscribeRoutesWithTopicString()
    {
        #region ibmmq-explicit-subscribe-topic-string

        var transport = new IBMMQTransport
        {
            // ... connection settings ...
        };

        // Subscribe using explicit topic strings
        transport.Topology.SubscribeTo<IOrderEvent>("prod/mycompany.events.orderplaced/");
        transport.Topology.SubscribeTo<IOrderEvent>("prod/mycompany.events.expressorderplaced/");

        #endregion
    }

    public void ExplicitPublishRoutes()
    {
        #region ibmmq-explicit-publish-routes

        var transport = new IBMMQTransport
        {
            // ... connection settings ...
        };

        // Override the default topic for a published event type
        transport.Topology.PublishTo<OrderPlaced>("legacy/order/events/");

        // When the admin object name can't be derived from the topic string
        transport.Topology.PublishTo<OrderPlaced>("LEGACY.ORDER.EVENTS", "legacy/order/events/");

        #endregion
    }

    public void DisablePolymorphicSubscriptionGuard()
    {
        #region ibmmq-disable-polymorphic-guard

        var transport = new IBMMQTransport
        {
            // ... connection settings ...
        };

        // Disable the guard that throws when subscribing to a type with descendants
        transport.Topology.ThrowOnPolymorphicSubscription = false;

        #endregion
    }

    public void CircuitBreaker()
    {
        #region ibmmq-circuit-breaker

        var transport = new IBMMQTransport
        {
            // ... connection settings ...
            TimeToWaitBeforeTriggeringCircuitBreaker = TimeSpan.FromMinutes(5)
        };

        #endregion
    }

    public void SendsAtomicWithReceive()
    {
        #region ibmmq-sends-atomic-with-receive

        var transport = new IBMMQTransport
        {
            // ... connection settings ...
            TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive
        };

        #endregion
    }

    public void ReceiveOnly()
    {
        #region ibmmq-receive-only

        var transport = new IBMMQTransport
        {
            // ... connection settings ...
            TransportTransactionMode = TransportTransactionMode.ReceiveOnly
        };

        #endregion
    }

    public void TransactionsNone()
    {
        #region ibmmq-transactions-none

        var transport = new IBMMQTransport
        {
            // ... connection settings ...
            TransportTransactionMode = TransportTransactionMode.None
        };

        #endregion
    }
}
