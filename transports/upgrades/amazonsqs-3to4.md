---
title: Upgrade AmazonSQS Transport Version 3 to 4
summary: Instructions on how to upgrade the AmazonSQS transport from version 3 to 4
component: SQS
isUpgradeGuide: true
reviewed: 2022-11-24
upgradeGuideCoreVersions:
 - 7
---

## MaxTTLDays renamed MaxTimeToLive

The `MaxTTLDays` method has been renamed to `MaxTimeToLive` and now takes a [TimeSpan](https://msdn.microsoft.com/en-us/library/system.timespan.aspx)

```csharp
var transport = endpointConfiguration.UseTransport<SqsTransport>();
transport.MaxTimeToLive(TimeSpan.FromDays(10));
```

## Region

To specify a region, set the `AWS_REGION` environment variable or overload the client factory.

```csharp
var transport = endpointConfiguration.UseTransport<SqsTransport>();
transport.ClientFactory(() => new AmazonSQSClient(
    new AmazonSQSConfig {
        RegionEndpoint = RegionEndpoint.APSoutheast2
    }));
```

### Credential source

The SDK credential source is picked up automatically.

```csharp
// picks up credentials as specified by the Amazon SDK
var transport = endpointConfiguration.UseTransport<SqsTransport>();
```

If desired, the credential source can be configured manually by overloading the client factory.

```csharp
var transport = endpointConfiguration.UseTransport<SqsTransport>();
// SqsCredentialSource.InstanceProfile
transport.ClientFactory(() => new AmazonSQSClient(new InstanceProfileAWSCredentials()));
// SqsCredentialSource.EnvironmentVariables
transport.ClientFactory(() => new AmazonSQSClient(new EnvironmentVariablesAWSCredentials()));
```

## Proxy

Previous versions automatically read the proxy username and password from `NSERVICEBUS_AMAZONSQS_PROXY_AUTHENTICATION_USERNAME` and `NSERVICEBUS_AMAZONSQS_PROXY_AUTHENTICATION_PASSWORD`, respectively. To achieve the same behavior, the client factory has to be overridden:

```csharp
var userName = Environment.GetEnvironmentVariable("NSERVICEBUS_AMAZONSQS_PROXY_AUTHENTICATION_USERNAME");
var password = Environment.GetEnvironmentVariable("NSERVICEBUS_AMAZONSQS_PROXY_AUTHENTICATION_PASSWORD");

var transport = endpointConfiguration.UseTransport<SqsTransport>();
transport.ClientFactory(() => new AmazonSQSClient(
    new AmazonSQSConfig {
        ProxyCredentials = new NetworkCredential(userName, password),
        ProxyHost = "127.0.0.1",
        ProxyPort = 8888
    }));
```

## S3 configuration

The ability to configure the S3 bucket and the key prefix has been moved to a dedicated configuration API for S3 related settings.

```csharp
var transport = endpointConfiguration.UseTransport<SqsTransport>();
var s3Configuration = transport.S3("bucketname", "keyprefix");
```

### Region

To specify a region, set the `AWS_REGION` environment variable or overload the client factory.

```csharp
var transport = endpointConfiguration.UseTransport<SqsTransport>();
var s3Configuration = transport.S3(bucketName, keyPrefix);
s3Configuration.ClientFactory(() => new AmazonS3Client(
    new AmazonS3Config {
        RegionEndpoint = RegionEndpoint.APSoutheast2
    }));
```

### Credential source

The SDK credential source is picked up automatically.

```csharp
        var transport = endpointConfiguration.UseTransport<SqsTransport>();

        // picks up credentials as specified by the Amazon SDK
        var s3Configuration = transport.S3(bucketName, keyPrefix);
```

If desired, the credential source can be configured manually by overloading the client factory.

```csharp
var transport = endpointConfiguration.UseTransport<SqsTransport>();
var s3Configuration = transport.S3(bucketName, keyPrefix);
// SqsCredentialSource.InstanceProfile
s3Configuration.ClientFactory(() => new AmazonS3Client(new InstanceProfileAWSCredentials()));
// or SqsCredentialSource.EnvironmentVariables
s3Configuration.ClientFactory(() => new AmazonS3Client(new EnvironmentVariablesAWSCredentials()));
```

### Proxy

Previous versions automatically read the proxy username and password from `NSERVICEBUS_AMAZONSQS_PROXY_AUTHENTICATION_USERNAME` and `NSERVICEBUS_AMAZONSQS_PROXY_AUTHENTICATION_PASSWORD`, respectively. To achieve the same behavior, the client factory has to be overridden:

```csharp
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
```

### Native deferral

The native deferral API has been deprecated because the transport does not use the timeout manager, so native deferral cannot be disabled.

### Unrestricted delayed delivery

By default, it is possible to send delays natively up to 15 min (900 seconds). A new unrestricted delayed delivery mechanism has been added to remove this restriction:

```csharp
var transport = endpointConfiguration.UseTransport<SqsTransport>();
transport.UnrestrictedDurationDelayedDelivery();
```

Consult the [delayed delivery documentation](/transports/sqs/delayed-delivery.md) for more information.

## Permissions

In addition to the previous permissions, the `GetQueueAttributes` permission is required to run SQS transport. The following permissions must be granted to run SQS transport.

### [SQS permissions](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-api-permissions-reference.html)

 * CreateQueue
 * DeleteMessage
 * DeleteMessageBatch
 * GetQueueUrl
 * ReceiveMessage
 * SendMessage
 * SendMessageBatch
 * SetQueueAttributes
 * GetQueueAttributes
 * ChangeMessageVisibility
 * ChangeMessageVisibilityBatch
 * PurgeQueue
