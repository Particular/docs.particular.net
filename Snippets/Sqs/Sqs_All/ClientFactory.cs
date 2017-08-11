namespace SqsAll
{
    using System;
    using System.Linq;
    using Amazon;
    using Amazon.CloudFormation;
    using Amazon.S3;
    using Amazon.SQS;

    static class ClientFactory
    {
        public static IAmazonSQS CreateSqsClient()
        {
            var config = new AmazonSQSConfig
            {
                RegionEndpoint = GetRegion()
            };


            return new AmazonSQSClient(config);
        }

        public static IAmazonS3 CreateS3Client()
        {
            var config = new AmazonS3Config
            {
                RegionEndpoint = GetRegion()
            };

            return new AmazonS3Client(config);
        }

        public static IAmazonCloudFormation CreateCloudFormationClient()
        {
            var config = new AmazonCloudFormationConfig
            {
                RegionEndpoint = GetRegion()
            };

            return new AmazonCloudFormationClient(config);
        }

        static RegionEndpoint GetRegion()
        {
            var region = EnvironmentHelper.GetEnvironmentVariable(SqsTransportConfigurationExtensions.RegionEnvironmentVariableName) ?? "ap-southeast-2";
            var awsRegion = RegionEndpoint.EnumerableAllRegions.SingleOrDefault(x => x.SystemName == region);

            if (awsRegion == null)
            {
                throw new ArgumentException($"Unknown region: \"{region}\"");
            }
            return awsRegion;
        }
    }
}