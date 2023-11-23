using Amazon.S3;
using NServiceBus;

class Upgrade
{
    void S3BucketForLargeMessages(EndpointConfiguration endpointConfiguration)
    {
        #region 6to7-S3

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
        #region 6to7-encryption

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