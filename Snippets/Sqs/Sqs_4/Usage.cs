using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region SqsTransport

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.Region("ap-southeast-2");
        transport.S3BucketForLargeMessages("myBucketName", "my/key/prefix");

        #endregion
    }

    void NativeDeferral(EndpointConfiguration endpointConfiguration)
    {
        #region NativeDeferral

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.NativeDeferral();

        #endregion
    }

    void CredentialSource(EndpointConfiguration endpointConfiguration)
    {
        #region CredentialSource

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.CredentialSource(SqsCredentialSource.InstanceProfile);

        #endregion
    }

    void MaxTTL(EndpointConfiguration endpointConfiguration)
    {
        #region MaxTTL

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.MaxTTLDays(10);

        #endregion
    }

    void QueueNamePrefix(EndpointConfiguration endpointConfiguration)
    {
        #region QueueNamePrefix

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.QueueNamePrefix("DEV-");

        #endregion
    }

    void Region(EndpointConfiguration endpointConfiguration)
    {
        #region QueueNamePrefix

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.Region("ap-southeast-2");

        #endregion
    }

    void S3BucketForLargeMessages(EndpointConfiguration endpointConfiguration)
    {
        #region S3BucketForLargeMessages

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.S3BucketForLargeMessages(
            s3BucketForLargeMessages: "ap-southeast-2",
            s3KeyPrefix: "my/sample/path");

        #endregion
    }

    void Proxy(EndpointConfiguration endpointConfiguration)
    {
        #region Proxy

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.Proxy(
            proxyHost: "127.0.0.1",
            proxyPort: 8888);

        #endregion
    }
}