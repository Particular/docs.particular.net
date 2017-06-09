using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region SqsTransport

        endpointConfiguration.UseTransport<SqsTransport>()
                .Region("ap-southeast-2")
                .S3BucketForLargeMessages("myBucketName", "my/key/prefix");
        
        #endregion
    }
}
