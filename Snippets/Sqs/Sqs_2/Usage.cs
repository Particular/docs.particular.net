using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region SqsTransport

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.Region("ap-southeast-2");
        transport.S3BucketForLargeMessages("myBucketName", "my/key/prefix");

        #endregion
    }
    void NativeDeferral(EndpointConfiguration endpointConfiguration)
    {
        #region NativeDeferral

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.NativeDeferral();

        #endregion
    }
}
