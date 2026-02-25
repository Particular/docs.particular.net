using System;
using NServiceBus;
using NServiceBus.Transport.IbmMq;

class Usage
{
    public void BasicConfig()
    {
        #region ibmmq-config-basic

        var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

        var transport = new IbmMqTransport(options =>
        {
            options.Host = "mq-server.example.com";
            options.Port = 1414;
            options.Channel = "DEV.APP.SVRCONN";
            options.QueueManagerName = "QM1";
            options.User = "app";
            options.Password = "passw0rd";
        });

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    public void BasicConnection()
    {
        #region ibmmq-basic-connection

        var transport = new IbmMqTransport(options =>
        {
            options.Host = "mq-server.example.com";
            options.Port = 1414;
            options.Channel = "DEV.APP.SVRCONN";
            options.QueueManagerName = "QM1";
        });

        #endregion
    }

    public void Authentication()
    {
        #region ibmmq-authentication

        var transport = new IbmMqTransport(options =>
        {
            options.Host = "mq-server.example.com";
            options.QueueManagerName = "QM1";
            options.User = "app";
            options.Password = "passw0rd";
        });

        #endregion
    }

    public void ApplicationName()
    {
        #region ibmmq-application-name

        var transport = new IbmMqTransport(options =>
        {
            // ... connection settings ...
            options.ApplicationName = "OrderService";
        });

        #endregion
    }

    public void HighAvailability()
    {
        #region ibmmq-high-availability

        var transport = new IbmMqTransport(options =>
        {
            options.QueueManagerName = "QM1";
            options.Channel = "APP.SVRCONN";
            options.Connections.Add("mqhost1(1414)");
            options.Connections.Add("mqhost2(1414)");
        });

        #endregion
    }

    public void SslTls()
    {
        #region ibmmq-ssl-tls

        var transport = new IbmMqTransport(options =>
        {
            options.Host = "mq-server.example.com";
            options.QueueManagerName = "QM1";
            options.SslKeyRepository = "*SYSTEM";
            options.CipherSpec = "TLS_RSA_WITH_AES_256_CBC_SHA256";
        });

        #endregion
    }

    public void SslPeerName()
    {
        #region ibmmq-ssl-peer-name

        var transport = new IbmMqTransport(options =>
        {
            // ... connection settings ...
            options.SslKeyRepository = "*SYSTEM";
            options.CipherSpec = "TLS_RSA_WITH_AES_256_CBC_SHA256";
            options.SslPeerName = "CN=MQSERVER01,O=MyCompany,C=US";
        });

        #endregion
    }

    public void CustomTopicPrefix()
    {
        #region ibmmq-custom-topic-prefix

        var transport = new IbmMqTransport(options =>
        {
            // ... connection settings ...
            options.TopicNaming = new TopicNaming("PROD");
        });

        #endregion
    }

    public void CustomTopicNamingUsage()
    {
        #region ibmmq-custom-topic-naming-usage

        var transport = new IbmMqTransport(options =>
        {
            // ... connection settings ...
            options.TopicNaming = new ShortTopicNaming();
        });

        #endregion
    }

    public void ResourceSanitization()
    {
        #region ibmmq-resource-sanitization

        var transport = new IbmMqTransport(options =>
        {
            // ... connection settings ...
            options.ResourceNameSanitizer = name =>
            {
                var sanitized = name.Replace("-", ".").Replace("/", ".");
                return sanitized.Length > 48 ? sanitized[..48] : sanitized;
            };
        });

        #endregion
    }

    public void PollingInterval()
    {
        #region ibmmq-polling-interval

        var transport = new IbmMqTransport(options =>
        {
            // ... connection settings ...
            options.MessageWaitInterval = TimeSpan.FromMilliseconds(2000);
        });

        #endregion
    }

    public void MaxMessageSize()
    {
        #region ibmmq-max-message-size

        var transport = new IbmMqTransport(options =>
        {
            // ... connection settings ...
            options.MaxMessageLength = 10 * 1024 * 1024; // 10 MB
        });

        #endregion
    }

    public void CharacterSet()
    {
        #region ibmmq-character-set

        var transport = new IbmMqTransport(options =>
        {
            // ... connection settings ...
            options.CharacterSet = 1208; // UTF-8 (default)
        });

        #endregion
    }

    public void SendsAtomicWithReceive()
    {
        #region ibmmq-sends-atomic-with-receive

        var transport = new IbmMqTransport(options =>
        {
            // ... connection settings ...
        });
        transport.TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive;

        #endregion
    }

    public void ReceiveOnly()
    {
        #region ibmmq-receive-only

        var transport = new IbmMqTransport(options =>
        {
            // ... connection settings ...
        });
        transport.TransportTransactionMode = TransportTransactionMode.ReceiveOnly;

        #endregion
    }

    public void TransactionsNone()
    {
        #region ibmmq-transactions-none

        var transport = new IbmMqTransport(options =>
        {
            // ... connection settings ...
        });
        transport.TransportTransactionMode = TransportTransactionMode.None;

        #endregion
    }
}
