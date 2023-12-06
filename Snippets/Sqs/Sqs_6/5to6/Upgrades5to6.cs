using Amazon.S3;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using NServiceBus;

class Upgrade
{
    void UsageNew(EndpointConfiguration endpointConfiguration)
    {
        #region 5to6-usage-new

        var transport = new SqsTransport();

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void UsageOld(EndpointConfiguration endpointConfiguration)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        #region 5to6-usage-old

        var transport = endpointConfiguration.UseTransport<SqsTransport>();

        #endregion
#pragma warning restore CS0618 // Type or member is obsolete
    }

    void ClientsNew(EndpointConfiguration endpointConfiguration)
    {
        #region 5to6-clients-new

        var transport = new SqsTransport(
            new AmazonSQSClient(),
            new AmazonSimpleNotificationServiceClient());

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void ClientsOld(EndpointConfiguration endpointConfiguration)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        #region 5to6-clients-old

        var transport = endpointConfiguration.UseTransport<SqsTransport>();

        transport.ClientFactory(() => new AmazonSQSClient());
        transport.ClientFactory(() => new AmazonSimpleNotificationServiceClient());

        #endregion
#pragma warning restore CS0618 // Type or member is obsolete
    }

    void S3BucketForLargeMessagesNew(EndpointConfiguration endpointConfiguration)
    {
        #region 5to6-S3-new

        var transport = new SqsTransport
        {
            S3 = new S3Settings(
                bucketForLargeMessages: "nsb-sqs-messages",
                keyPrefix: "my/sample/path",
                s3Client: new AmazonS3Client())
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void S3BucketForLargeMessagesOld(EndpointConfiguration endpointConfiguration)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        #region 5to6-S3-old

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        var s3Configuration = transport.S3("nsb-sqs-messages", "my/sample/path");

        s3Configuration.ClientFactory(() => new AmazonS3Client());

        #endregion
#pragma warning restore CS0618 // Type or member is obsolete
    }

    void S3ServerSideEncryptionNew(EndpointConfiguration endpointConfiguration, string bucketName, string keyPrefix)
    {
        #region 5to6-encryption-new

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

    void S3ServerSideEncryptionOld(EndpointConfiguration endpointConfiguration, string bucketName, string keyPrefix)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        #region 5to6-encryption-old

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        var s3Configuration = transport.S3(bucketName, keyPrefix);

        s3Configuration.ServerSideEncryption(ServerSideEncryptionMethod.AES256, keyManagementServiceKeyId: "MyKeyId");

        #endregion
#pragma warning restore CS0618 // Type or member is obsolete
    }
}