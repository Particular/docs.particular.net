namespace SqsAll
{
    using Amazon.CloudFormation;
    using Amazon.S3;
    using Amazon.SimpleNotificationService;
    using Amazon.SQS;

    static class ClientFactory
    {
        public static IAmazonSQS CreateSqsClient()
        {
            return new AmazonSQSClient();
        }

        public static IAmazonSimpleNotificationService CreateSnsClient() 
        {
            return new AmazonSimpleNotificationServiceClient();
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