namespace SqsAll
{
    using Amazon.CloudFormation;
    using Amazon.S3;
    using Amazon.SQS;

    static class ClientFactory
    {
        public static IAmazonSQS CreateSqsClient()
        {
            return new AmazonSQSClient();
        }

        public static IAmazonS3 CreateS3Client()
        {
            return new AmazonS3Client();
        }

        public static IAmazonCloudFormation CreateCloudFormationClient()
        {
            return new AmazonCloudFormationClient();
        }
    }
}