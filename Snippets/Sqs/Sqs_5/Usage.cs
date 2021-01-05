using System;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SQS;
using NServiceBus;
using Amazon.SQS.Model;
using NServiceBus.Pipeline;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region SqsTransport

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        // S3 bucket only required for messages larger than 256KB
        var s3Configuration = transport.S3("myBucketName", "my/key/prefix");

        #endregion
    }

    void DelayedDelivery(EndpointConfiguration endpointConfiguration)
    {
        #region DelayedDelivery

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.UnrestrictedDurationDelayedDelivery();

        #endregion
    }

    void CredentialSource(EndpointConfiguration endpointConfiguration)
    {
        #region CredentialSource

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.ClientFactory(() => new AmazonSQSClient(new InstanceProfileAWSCredentials()));

        #endregion
    }

    void S3CredentialSource(EndpointConfiguration endpointConfiguration, string bucketName, string keyPrefix)
    {
        #region S3CredentialSource

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        var s3Configuration = transport.S3(bucketName, keyPrefix);
        s3Configuration.ClientFactory(() => new AmazonS3Client(new InstanceProfileAWSCredentials()));

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
        transport.ClientFactory(() => new AmazonSQSClient(
            new AmazonSQSConfig {
                RegionEndpoint = RegionEndpoint.APSoutheast2
            }));

        #endregion
    }

    void S3Region(EndpointConfiguration endpointConfiguration, string bucketName, string keyPrefix)
    {
        #region S3Region

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        var s3Configuration = transport.S3(bucketName, keyPrefix);
        s3Configuration.ClientFactory(() => new AmazonS3Client(
            new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.APSoutheast2
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
        var s3Configuration = transport.S3(
            bucketForLargeMessages: "nsb-sqs-messages",
            keyPrefix: "my/sample/path");

        #endregion
    }

    void S3ClientFactory(EndpointConfiguration endpointConfiguration, string bucketName, string keyPrefix)
    {
        #region S3ClientFactory

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        var s3Configuration = transport.S3(bucketName, keyPrefix);
        s3Configuration.ClientFactory(() => new AmazonS3Client(new AmazonS3Config()));

        #endregion
    }

    void S3ServerSideEncryption(EndpointConfiguration endpointConfiguration, string bucketName, string keyPrefix)
    {
        #region S3ServerSideEncryption

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        var s3Configuration = transport.S3(bucketName, keyPrefix);
        s3Configuration.ServerSideEncryption(ServerSideEncryptionMethod.AES256, keyManagementServiceKeyId: "keyId");

        #endregion
    }

    void S3ServerSideCustomerEncryption(EndpointConfiguration endpointConfiguration, string bucketName, string keyPrefix)
    {
        #region S3ServerSideCustomerEncryption

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        var s3Configuration = transport.S3(bucketName, keyPrefix);
        s3Configuration.ServerSideCustomerEncryption(ServerSideEncryptionCustomerMethod.AES256, "key", providedKeyMD5: "keyMD5");

        #endregion
    }

    void Proxy(EndpointConfiguration endpointConfiguration, string userName, string password)
    {
        #region Proxy

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.ClientFactory(() => new AmazonSQSClient(
            new AmazonSQSConfig {
                ProxyCredentials = new NetworkCredential(userName, password),
                ProxyHost = "127.0.0.1",
                ProxyPort = 8888
            }));

        #endregion
    }

    void S3Proxy(EndpointConfiguration endpointConfiguration, string bucketName, string keyPrefix, string userName, string password)
    {
        #region S3Proxy

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        var s3Configuration = transport.S3(bucketName, keyPrefix);
        s3Configuration.ClientFactory(() => new AmazonS3Client(
            new AmazonS3Config
            {
                ProxyCredentials = new NetworkCredential(userName, password),
                ProxyHost = "127.0.0.1",
                ProxyPort = 8888
            }));

        #endregion
    }

    void V1BackwardsCompatibility(EndpointConfiguration endpointConfiguration)
    {
        #region V1BackwardsCompatibility

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.EnableV1CompatibilityMode();

        #endregion
    }

    void CustomTopicsMappingsTypeToType(EndpointConfiguration endpointConfiguration)
    {
        #region CustomTopicsMappingsTypeToType

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.MapEvent<SubscribedEvent, PublishedEvent>();

        #endregion
    }

    void CustomTopicsMappingsTypeToTopic(EndpointConfiguration endpointConfiguration)
    {
        #region CustomTopicsMappingsTypeToTopic

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.MapEvent<SubscribedEvent>("topic-used-by-the-publisher");

        #endregion
    }

    void CustomTopicsMappingsTypeToTopicForTopology(EndpointConfiguration endpointConfiguration)
    {
        #region CustomTopicsMappingsTypeToTopicForTopology

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.MapEvent<IOrderAccepted>("namespace-OrderAccepted");

        #endregion
    }

    void EnableMessageDrivenPubSubCompatibilityMode(EndpointConfiguration endpointConfiguration)
    {
#pragma warning disable 618
        #region EnableMessageDrivenPubSubCompatibilityMode

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.EnableMessageDrivenPubSubCompatibilityMode();

        #endregion
#pragma warning restore 618
    }

    void TopicNamePrefix(EndpointConfiguration endpointConfiguration)
    {
        #region TopicNamePrefix

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.TopicNamePrefix("DEV-");

        #endregion
    }

    void TopicNameGenerator(EndpointConfiguration endpointConfiguration)
    {
        #region TopicNameGenerator

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.TopicNameGenerator((eventType, topicNamePrefix) => $"{topicNamePrefix}{eventType.Name}");

        #endregion
    }

    class SubscribedEvent { }

    class PublishedEvent { }

    interface IOrderAccepted { }

    class OrderAccepted : IOrderAccepted{ }
}

#region sqs-access-to-native-message
class AccessToAmazonSqsNativeMessage : Behavior<IIncomingContext>
{
    public override Task Invoke(IIncomingContext context, Func<Task> next)
    {
        // get the native Amazon SQS message
        var message = context.Extensions.Get<Message>();

        //do something useful

        return next();
    }
}
#endregion