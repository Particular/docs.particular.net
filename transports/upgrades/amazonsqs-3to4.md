---
title: Upgrade AmazonSQS Transport Version 3 to 4
summary: Instructions on how to upgrade the AmazonSQS transport from version 3 to 4
component: SQS
isUpgradeGuide: true
reviewed: 2018-05-22
upgradeGuideCoreVersions:
 - 7
---

## MaxTTLDays renamed MaxTimeToLive

The `MaxTTLDays` method has been renamed to `MaxTimeToLive` and now takes a [TimeSpan](https://msdn.microsoft.com/en-us/library/system.timespan.aspx)

snippet: 3to4_MaxTTL

## Region

To specify a region set the `AWS_REGION` environment variable or overload the client factory.

snippet: 3to4_Region

### Credential source

The SDK credential source is picked up automatically.

snippet: 3to4_CredentialSource

If desired the credential source can be configured manually by overloading the client factory.

snippet: 3to4_CredentialSourceManual

## Proxy

Previous versions automatically read the proxy username and password from `NSERVICEBUS_AMAZONSQS_PROXY_AUTHENTICATION_USERNAME` and `NSERVICEBUS_AMAZONSQS_PROXY_AUTHENTICATION_PASSWORD` respectively. To achieve the same behavior the client factory has to be overridden:

snippet: 3to4_Proxy

## S3 configuration

The possibility to configure the S3 bucket and the key prefix has been moved to a dedicated configuration API for S3 related settings.

snippet: 3to4_S3BucketForLargeMessages

### Region

To specify a region set the `AWS_REGION` environment variable or overload the client factory.

snippet: 3to4_S3Region

### Credential source

The SDK credential source is picked up automatically.

snippet: 3to4_S3CredentialSource

If desired the credential source can be configured manually by overloading the client factory.

snippet: 3to4_S3CredentialSourceManual

### Proxy

Previous versions automatically read the proxy username and password from `NSERVICEBUS_AMAZONSQS_PROXY_AUTHENTICATION_USERNAME` and `NSERVICEBUS_AMAZONSQS_PROXY_AUTHENTICATION_PASSWORD` respectively. To achieve the same behavior the client factory has to be overridden:

snippet: 3to4_S3Proxy

### Native deferral

The native deferral API has been deprecated because the transport does not use the timeout manager, so native deferral cannot be disabled.

### Unrestricted delayed delivery

By default, it is possible to send delays natively up to 15 min (900 seconds). A new unrestricted delayed delivery mechanism has been added to remove this restriction:

snippet: 3to4_DelayedDelivery

Consult the [delayed delivery documentation](/transports/sqs/delayed-delivery.md) for more information.

## Permissions

In addition to the previous permissions, the `GetQueueAttributes` permission is required to run SQS transport. The following permissions must be granted to run SQS transport.

### [SQS permissions](http://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-api-permissions-reference.html)

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