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

    void S3BucketForLargeMessages(EndpointConfiguration endpointConfiguration)
    {
        #region 5to6-S3

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

    void S3ServerSideEncryption(EndpointConfiguration endpointConfiguration, string bucketName, string keyPrefix)
    {
        #region 5to6-encryption

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
}