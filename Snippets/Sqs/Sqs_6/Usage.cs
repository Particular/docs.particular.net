using System;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Amazon.SQS.Model;
using NServiceBus;
using NServiceBus.Pipeline;


class Usage
{
    void AssumePermissionsOnPolicy(EndpointConfiguration config)
    {
        #region assume-permissions

        var transport = new SqsTransport();
        
        transport.Policies.SetupTopicPoliciesWhenSubscribing = false;
        
        config.UseTransport(transport);

        #endregion
    }

    void WildcardsAccount(EndpointConfiguration config)
    {
        #region wildcard-account-condition

        var transport = new SqsTransport();

        transport.Policies.AccountCondition = true;

        config.UseTransport(transport);

        #endregion
    }

    void WildcardsPrefix(EndpointConfiguration config)
    {
        #region wildcard-prefix-condition

        var transport = new SqsTransport();

        transport.Policies.TopicNamePrefixCondition = true;

        config.UseTransport(transport);

        #endregion
    }

    void WildcardsNamespace(EndpointConfiguration config)
    {
        #region wildcard-namespace-condition

        var transport = new SqsTransport();

        transport.Policies.TopicNamespaceConditions
            .Add("Sales.");
        transport.Policies.TopicNamespaceConditions
            .Add("Shipping.HighValueOrders.");

        config.UseTransport(transport);

        #endregion
    }

    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region SqsTransport

        // S3 bucket only required for messages larger than 256KB
        var transport = new SqsTransport
        {
            S3 = new S3Settings("myBucketName", "my/key/prefix")
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void CredentialSource(EndpointConfiguration endpointConfiguration)
    {
        #region CredentialSource

        var transport = new SqsTransport(
            new AmazonSQSClient(new InstanceProfileAWSCredentials()), 
            new AmazonSimpleNotificationServiceClient());

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void S3CredentialSource(EndpointConfiguration endpointConfiguration, string bucketName, string keyPrefix)
    {
        #region S3CredentialSource

        var transport = new SqsTransport
        {
            S3 = new S3Settings(bucketName, keyPrefix,
                new AmazonS3Client(new InstanceProfileAWSCredentials()))
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void MaxTTL(EndpointConfiguration endpointConfiguration)
    {
        #region MaxTTL

        var transport = new SqsTransport
        {
            MaxTimeToLive = TimeSpan.FromDays(10)
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void QueueNamePrefix(EndpointConfiguration endpointConfiguration)
    {
        #region QueueNamePrefix

        var transport = new SqsTransport
        {
            QueueNamePrefix = "DEV-"
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void Region(EndpointConfiguration endpointConfiguration)
    {
        #region Region

        var transport = new SqsTransport(new AmazonSQSClient(
            new AmazonSQSConfig
            {
                RegionEndpoint = RegionEndpoint.APSoutheast2
            }), 
            new AmazonSimpleNotificationServiceClient());

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void S3Region(EndpointConfiguration endpointConfiguration, string bucketName, string keyPrefix)
    {
        #region S3Region

        var transport = new SqsTransport
        {
            S3 = new S3Settings(bucketName, keyPrefix,
                new AmazonS3Client(new AmazonS3Config
                {
                    RegionEndpoint = RegionEndpoint.APSoutheast2
                }))
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void ClientFactory(EndpointConfiguration endpointConfiguration)
    {
        #region ClientFactory

        var transport = new SqsTransport(
            new AmazonSQSClient(new AmazonSQSConfig()),
            new AmazonSimpleNotificationServiceClient());

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void S3BucketForLargeMessages(EndpointConfiguration endpointConfiguration)
    {
        #region S3BucketForLargeMessages

        var transport = new SqsTransport
        {
            S3 = new S3Settings(
                bucketForLargeMessages: "nsb-sqs-messages",
                keyPrefix: "my/sample/path")
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void S3ClientFactory(EndpointConfiguration endpointConfiguration, string bucketName, string keyPrefix)
    {
        #region S3ClientFactory

        var transport = new SqsTransport
        {
            S3 = new S3Settings(bucketName, keyPrefix,
                new AmazonS3Client(new AmazonS3Config()))
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void S3ServerSideEncryption(EndpointConfiguration endpointConfiguration, string bucketName, string keyPrefix)
    {
        #region S3ServerSideEncryption

        var transport = new SqsTransport
        {
            S3 = new S3Settings(bucketName, keyPrefix)
            {
                Encryption = new S3EncryptionWithManagedKey(ServerSideEncryptionMethod.AES256, "keyId")
            }
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void S3ServerSideCustomerEncryption(EndpointConfiguration endpointConfiguration, string bucketName, string keyPrefix)
    {
        #region S3ServerSideCustomerEncryption

        var transport = new SqsTransport
        {
            S3 = new S3Settings(bucketName, keyPrefix)
            {
                Encryption = new S3EncryptionWithCustomerProvidedKey(ServerSideEncryptionCustomerMethod.AES256, "key", "keyMD5")
            }
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void Proxy(EndpointConfiguration endpointConfiguration, string userName, string password)
    {
        #region Proxy

        var transport = new SqsTransport(new AmazonSQSClient(
                new AmazonSQSConfig
                {
                    ProxyCredentials = new NetworkCredential(userName, password),
                    ProxyHost = "127.0.0.1",
                    ProxyPort = 8888
                }),
            new AmazonSimpleNotificationServiceClient());

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void S3Proxy(EndpointConfiguration endpointConfiguration, string bucketName, string keyPrefix, string userName, string password)
    {
        #region S3Proxy

        var transport = new SqsTransport
        {
            S3 = new S3Settings(bucketName, keyPrefix,
                new AmazonS3Client(new AmazonS3Config
                {
                    ProxyCredentials = new NetworkCredential(userName, password),
                    ProxyHost = "127.0.0.1",
                    ProxyPort = 8888
                }))
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void V1BackwardsCompatibility(EndpointConfiguration endpointConfiguration)
    {
        #region V1BackwardsCompatibility

        var transport = new SqsTransport
        {
            EnableV1CompatibilityMode = true
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void CustomTopicsMappingsTypeToType(EndpointConfiguration endpointConfiguration)
    {
        #region CustomTopicsMappingsTypeToType

        var transport = new SqsTransport();

        transport.MapEvent<SubscribedEvent, PublishedEvent>();

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void CustomTopicsMappingsTypeToTopic(EndpointConfiguration endpointConfiguration)
    {
        #region CustomTopicsMappingsTypeToTopic

        var transport = new SqsTransport();

        transport.MapEvent<SubscribedEvent>("topic-used-by-the-publisher");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void CustomTopicsMappingsTypeToTopicForTopology(EndpointConfiguration endpointConfiguration)
    {
        #region CustomTopicsMappingsTypeToTopicForTopology

        var transport = new SqsTransport();

        transport.MapEvent<IOrderAccepted>("namespace-OrderAccepted");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }


    void TopicNamePrefix(EndpointConfiguration endpointConfiguration)
    {
        #region TopicNamePrefix

        var transport = new SqsTransport
        {
            TopicNamePrefix = "DEV-"
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void TopicNameGenerator(EndpointConfiguration endpointConfiguration)
    {
        #region TopicNameGenerator

        var transport = new SqsTransport
        {
            TopicNameGenerator = (eventType, topicNamePrefix) => $"{topicNamePrefix}{eventType.Name}"
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    class SubscribedEvent { }

    class PublishedEvent { }

    interface IOrderAccepted { }

    class OrderAccepted : IOrderAccepted { }
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