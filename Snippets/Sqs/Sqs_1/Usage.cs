using NServiceBus;

class Usage
{
    Usage(BusConfiguration busConfiguration)
    {
        #region SqsTransport

        var transport = busConfiguration.UseTransport<SqsTransport>();
        transport.ConnectionString("Region=ap-southeast-2;S3BucketForLargeMessages=myBucketName;S3KeyPrefix=my/key/prefix;");
        
        #endregion
    }
}
