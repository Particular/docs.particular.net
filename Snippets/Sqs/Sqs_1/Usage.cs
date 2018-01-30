using NServiceBus;

class Usage
{
    Usage(BusConfiguration busConfiguration)
    {
        #region SqsTransport

        var transport = busConfiguration.UseTransport<SqsTransport>();
        // S3 bucket only required for messages larger than 256KB
        transport.ConnectionString("Region=ap-southeast-2;S3BucketForLargeMessages=myBucketName;S3KeyPrefix=my/key/prefix;");

        #endregion
    }

    void CredentialSource(BusConfiguration busConfiguration)
    {
        #region CredentialSource

        var transport = busConfiguration.UseTransport<SqsTransport>();
        transport.ConnectionString("CredentialSource=InstanceProfile;");

        #endregion
    }

    void MaxTTL(BusConfiguration busConfiguration)
    {
        #region MaxTTL

        var transport = busConfiguration.UseTransport<SqsTransport>();
        transport.ConnectionString("MaxTTLDays=1;");

        #endregion
    }

    void QueueNamePrefix(BusConfiguration busConfiguration)
    {
        #region QueueNamePrefix

        var transport = busConfiguration.UseTransport<SqsTransport>();
        transport.ConnectionString("QueueNamePrefix=DEV-;");

        #endregion
    }

    void Region(BusConfiguration busConfiguration)
    {
        #region Region

        var transport = busConfiguration.UseTransport<SqsTransport>();
        transport.ConnectionString("Region=ap-southeast-2;");

        #endregion
    }

    void S3BucketForLargeMessages(BusConfiguration busConfiguration)
    {
        #region S3BucketForLargeMessages

        var transport = busConfiguration.UseTransport<SqsTransport>();
        transport.ConnectionString("S3BucketForLargeMessages=nsb-sqs-messages");

        #endregion
    }

    void Proxy(BusConfiguration busConfiguration)
    {
        #region Proxy

        var transport = busConfiguration.UseTransport<SqsTransport>();
        transport.ConnectionString("ProxyHost=127.0.0.1;ProxyPort=8888;");

        #endregion
    }

    void MaxReceiveMessageBatchSize(BusConfiguration busConfiguration)
    {
        #region MaxReceiveMessageBatchSize

        var transport = busConfiguration.UseTransport<SqsTransport>();
        transport.ConnectionString("MaxReceiveMessageBatchSize=1;");

        #endregion
    }
}
