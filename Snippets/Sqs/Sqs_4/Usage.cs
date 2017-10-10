using System;
using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region SqsTransport

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        var s3Configuration = transport.S3();
        s3Configuration.BucketForLargeMessages("myBucketName", "my/key/prefix");

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
        transport.ClientFactory(() => new AmazonSQSClient(new InstanceProfileAWSCredentials()));

        #endregion
    }

    void MaxTTL(EndpointConfiguration endpointConfiguration)
    {
        #region MaxTTL

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.MaxTimeToLive(TimeSpan.FromDays(10));

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
        #region Region

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        // set AWS_REGION environment variable or override client factory
        transport.ClientFactory(() => new AmazonSQSClient(
            new AmazonSQSConfig { 
                Region = "ap-southeast-2"
            }));

        #endregion
    }

    void ClientFactory(EndpointConfiguration endpointConfiguration)
    {
        #region ClientFactory

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.ClientFactory(() => new AmazonSQSClient(new AmazonSQSConfig()));

        #endregion
    }    

    void S3BucketForLargeMessages(EndpointConfiguration endpointConfiguration)
    {
        #region S3BucketForLargeMessages

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        var s3Configuration = transport.S3();
        s3Configuration.BucketForLargeMessages(
            s3BucketForLargeMessages: "ap-southeast-2",
            s3KeyPrefix: "my/sample/path");

        #endregion
    }

    void S3ClientFactory(EndpointConfiguration endpointConfiguration)
    {
        #region S3ClientFactory

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        var s3Configuration = transport.S3();
        s3Configuration.ClientFactory(() => new AmazonS3Client(new AmazonS3Config()));

        #endregion
    }    

    void Proxy(EndpointConfiguration endpointConfiguration, string userName, string password)
    {
        #region Proxy

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.ClientFactory(() => new AmazonSQSClient(
            new AmazonSQSConfig { 
                ProxyCredentials = new NetworkCredentials(userName, password),
                ProxyHost = "127.0.0.1", 
                ProxyPort = 8888 
            }));

        #endregion
    }
}