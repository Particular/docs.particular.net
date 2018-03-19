namespace SqsAll
{
    using Amazon;
    using Amazon.SQS;
    using NServiceBus;

    public static class SqsTransportConfigurationExtensions
    {
        internal const string RegionEnvironmentVariableName = "NServiceBus_AmazonSQS_Region";
        const string S3BucketEnvironmentVariableName = "NServiceBus_AmazonSQS_S3Bucket";
        const string NativeDeferralEnvironmentVariableName = "NServiceBus_AmazonSQS_NativeDeferral";

        public static string S3BucketName => EnvironmentHelper.GetEnvironmentVariable(S3BucketEnvironmentVariableName);

        public static void ConfigureSqsTransport(this TransportExtensions<SqsTransport> transportConfiguration, string queueNamePrefix = null)
        {
            var region = EnvironmentHelper.GetEnvironmentVariable(RegionEnvironmentVariableName);

            transportConfiguration.ClientFactory(() => new AmazonSQSClient(new AmazonSQSConfig { RegionEndpoint = RegionEndpoint.GetBySystemName(region) }));
            if (queueNamePrefix != null)
            {
                transportConfiguration.QueueNamePrefix(queueNamePrefix);
            }
                
            var s3BucketName = EnvironmentHelper.GetEnvironmentVariable(S3BucketEnvironmentVariableName);

            if (!string.IsNullOrEmpty(S3BucketName))
            {
                transportConfiguration.S3(S3BucketName, "test");
            }

            var nativeDeferralRaw = EnvironmentHelper.GetEnvironmentVariable(NativeDeferralEnvironmentVariableName);
            var validValue = bool.TryParse(nativeDeferralRaw, out var nativeDeferral);
            if (validValue && nativeDeferral)
            {
#pragma warning disable 618
                transportConfiguration.NativeDeferral();
#pragma warning restore 618
            }
        }
    }
}