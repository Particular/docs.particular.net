namespace Sqs_All
{
    using System;
    using System.Linq;
    using Amazon;
    using Amazon.Runtime;
    using Amazon.SQS;

    static class ClientFactory
    {
        public static IAmazonSQS CreateSqsClient()
        {
            var region = EnvironmentHelper.GetEnvironmentVariable(SqsTransportConfigurationExtensions.RegionEnvironmentVariableName) ?? "ap-southeast-2";
            var awsRegion = RegionEndpoint.EnumerableAllRegions.SingleOrDefault(x => x.SystemName == region);

            if (awsRegion == null)
            {
                throw new ArgumentException($"Unknown region: \"{region}\"");
            }

            var config = new AmazonSQSConfig
            {
                RegionEndpoint = awsRegion
            };


            return new AmazonSQSClient(new EnvironmentVariablesAWSCredentials(), config);
        }
    }
}