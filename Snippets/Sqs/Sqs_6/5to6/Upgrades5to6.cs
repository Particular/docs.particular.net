using Amazon.S3;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using NServiceBus;

class Upgrade
{
    void Clients(EndpointConfiguration endpointConfiguration)
    {
        #region 5to6-clients

        var transport = new SqsTransport(
            new AmazonSQSClient(),
            new AmazonSimpleNotificationServiceClient());

        endpointConfiguration.UseTransport(transport);

        #endregion
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
        s3Configuration.ClientFactory(() => new AmazonS3Client(new AmazonS3Config()));

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