using System;
using System.Net;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SQS;
using NServiceBus;

class Upgrade3to4
{
    void MaxTTL(EndpointConfiguration endpointConfiguration)
    {
        #region 3to4_MaxTTL

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.MaxTimeToLive(TimeSpan.FromDays(10));

        #endregion
    }

    void BucketForLargeMessages(EndpointConfiguration endpointConfiguration)
    {
        #region 3to4_S3BucketForLargeMessages

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        var s3Configuration = transport.S3("bucketname", "keyprefix");

        #endregion
    }

    void S3Proxy(EndpointConfiguration endpointConfiguration, string bucketName, string keyPrefix)
    {
        #region 3to4_S3Proxy

        var userName = Environment.GetEnvironmentVariable("NSERVICEBUS_AMAZONSQS_PROXY_AUTHENTICATION_USERNAME");
        var password = Environment.GetEnvironmentVariable("NSERVICEBUS_AMAZONSQS_PROXY_AUTHENTICATION_PASSWORD");

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        var s3Configuration = transport.S3(bucketName, keyPrefix);
        s3Configuration.ClientFactory(() => new AmazonS3Client(
            new AmazonS3Config { 
                ProxyCredentials = new NetworkCredential(userName, password),
                ProxyHost = "127.0.0.1", 
                ProxyPort = 8888 
            }));

        #endregion
    }

    void S3Region(EndpointConfiguration endpointConfiguration, string bucketName, string keyPrefix)
    {
        #region 3to4_S3Region

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        var s3Configuration = transport.S3(bucketName, keyPrefix);
        s3Configuration.ClientFactory(() => new AmazonS3Client(
            new AmazonS3Config { 
                RegionEndpoint = RegionEndpoint.APSoutheast2
            }));

        #endregion
    }

    void S3CredentialSource(EndpointConfiguration endpointConfiguration, string bucketName, string keyPrefix)
    {
        #region 3to4_S3CredentialSource

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        
        // picks up credentials as specified by the Amazon SDK
        var s3Configuration = transport.S3(bucketName, keyPrefix);

        #endregion
    }

    void S3CredentialSourceManual(EndpointConfiguration endpointConfiguration, string bucketName, string keyPrefix)
    {
        #region 3to4_S3CredentialSourceManual

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        var s3Configuration = transport.S3(bucketName, keyPrefix);
        // SqsCredentialSource.InstanceProfile
        s3Configuration.ClientFactory(() => new AmazonS3Client(new InstanceProfileAWSCredentials()));
        // or SqsCredentialSource.EnvironmentVariables
        s3Configuration.ClientFactory(() => new AmazonS3Client(new EnvironmentVariablesAWSCredentials()));

        #endregion
    }    

    void Proxy(EndpointConfiguration endpointConfiguration)
    {
        #region 3to4_Proxy

        var userName = Environment.GetEnvironmentVariable("NSERVICEBUS_AMAZONSQS_PROXY_AUTHENTICATION_USERNAME");		
        var password = Environment.GetEnvironmentVariable("NSERVICEBUS_AMAZONSQS_PROXY_AUTHENTICATION_PASSWORD");

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.ClientFactory(() => new AmazonSQSClient(
            new AmazonSQSConfig { 
                ProxyCredentials = new NetworkCredential(userName, password),
                ProxyHost = "127.0.0.1", 
                ProxyPort = 8888 
            }));

        #endregion
    }

    void Region(EndpointConfiguration endpointConfiguration)
    {
        #region 3to4_Region

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.ClientFactory(() => new AmazonSQSClient(
            new AmazonSQSConfig {
                RegionEndpoint = RegionEndpoint.APSoutheast2
            }));

        #endregion
    }

    void CredentialSource(EndpointConfiguration endpointConfiguration)
    {
        #region 3to4_CredentialSource

        // picks up credentials as specified by the Amazon SDK
        var transport = endpointConfiguration.UseTransport<SqsTransport>();

        #endregion
    }
    void CredentialSourceManual(EndpointConfiguration endpointConfiguration)
    {
        #region 3to4_CredentialSourceManual

        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        // SqsCredentialSource.InstanceProfile
        transport.ClientFactory(() => new AmazonSQSClient(new InstanceProfileAWSCredentials()));
        // SqsCredentialSource.EnvironmentVariables
        transport.ClientFactory(() => new AmazonSQSClient(new EnvironmentVariablesAWSCredentials()));

        #endregion
    }    
}