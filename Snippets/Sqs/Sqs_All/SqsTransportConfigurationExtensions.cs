namespace SqsAll
{
    using NServiceBus;

    public static class SqsTransportConfigurationExtensions
    {
        const string S3BucketEnvironmentVariableName = "NServiceBus_AmazonSQS_S3Bucket";

        public static string S3BucketName => EnvironmentHelper.GetEnvironmentVariable(S3BucketEnvironmentVariableName);

        public static void ConfigureSqsTransport(this TransportExtensions<SqsTransport> transportConfiguration, string queueNamePrefix = null)
        {
            transportConfiguration.ClientFactory(()=> ClientFactory.CreateSqsClient());
            transportConfiguration.ClientFactory(() => ClientFactory.CreateSnsClient());
            if (queueNamePrefix != null)
            {
                transportConfiguration.QueueNamePrefix(queueNamePrefix);
            }
                
            var s3BucketName = EnvironmentHelper.GetEnvironmentVariable(S3BucketEnvironmentVariableName);

            if (!string.IsNullOrEmpty(S3BucketName))
            {
                transportConfiguration.S3(S3BucketName, "test");
            }
        }
    }
}