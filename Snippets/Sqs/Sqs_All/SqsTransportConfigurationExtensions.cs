namespace Sqs_All
{
    using NServiceBus;

    public static class SqsTransportConfigurationExtensions
    {
        internal const string RegionEnvironmentVariableName = "NServiceBus.AmazonSQS.Region";
        const string S3BucketEnvironmentVariableName = "NServiceBus.AmazonSQS.S3Bucket";
        const string NativeDeferralEnvironmentVariableName = "NServiceBus.AmazonSQS.NativeDeferral";

        public static TransportExtensions<SqsTransport> ConfigureSqsTransport(this TransportExtensions<SqsTransport> transportConfiguration, string queueNamePrefix)
        {
            var region = EnvironmentHelper.GetEnvironmentVariable(RegionEnvironmentVariableName) ?? "ap-southeast-2";

            transportConfiguration
                .Region(region)
                .QueueNamePrefix(queueNamePrefix);

            var s3BucketName = EnvironmentHelper.GetEnvironmentVariable(S3BucketEnvironmentVariableName);

            if (!string.IsNullOrEmpty(s3BucketName))
            {
                transportConfiguration.S3BucketForLargeMessages(s3BucketName, "test");
            }

            var nativeDeferralRaw = EnvironmentHelper.GetEnvironmentVariable(NativeDeferralEnvironmentVariableName);
            var validValue = bool.TryParse(nativeDeferralRaw, out var nativeDeferral);
            if (validValue && nativeDeferral)
            {
                transportConfiguration.NativeDeferral();
            }

            return transportConfiguration;
        }
    }
}