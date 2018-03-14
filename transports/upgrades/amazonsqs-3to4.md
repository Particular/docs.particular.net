---
title: Upgrade AmazonSQS Transport Version 3 to 4
summary: Instructions on how to upgrade AmazonSQS Transport Version 3 to 4
component: SQS
isUpgradeGuide: true
reviewed: 2017-10-12
upgradeGuideCoreVersions:
 - 7
---

## MaxTTLDays renamed MaxTimeToLive

The `MaxTTLDays` method has been renamed to `MaxTimeToLive` and now takes a [TimeSpan](https://msdn.microsoft.com/en-us/library/system.timespan.aspx)

snippet: 3to4_MaxTTL

## Region

To specify a region set the `AWS_REGION` environment variable or overload the client factory.

snippet: 3to4_Region

### Credential Source

The SDK credential source is picked up automatically.

snippet: 3to4_CredentialSource

If desired the credential source can be configured manually by overloading the client factory.

snippet: 3to4_CredentialSourceManual

## Proxy

Previous versions automatically read the proxy username and password from `NSERVICEBUS_AMAZONSQS_PROXY_AUTHENTICATION_USERNAME` and `NSERVICEBUS_AMAZONSQS_PROXY_AUTHENTICATION_PASSWORD` respectively. To achieve the same behavior the client factory has to be overriden like shown below.

snippet: 3to4_Proxy

## S3 Configuration

The possibility to configure the S3 bucket and the key prefix has been moved to a dedicated configuration API for S3 related settings.

snippet: 3to4_S3BucketForLargeMessages

### Region

To specify a region set the `AWS_REGION` environment variable or overload the client factory.

snippet: 3to4_S3Region

### Credential Source

The SDK credential source is picked up automatically.

snippet: 3to4_S3CredentialSource

If desired the credential source can be configured manually by overloading the client factory.

snippet: 3to4_S3CredentialSourceManual

### Proxy

Previous versions automatically read the proxy username and password from `NSERVICEBUS_AMAZONSQS_PROXY_AUTHENTICATION_USERNAME` and `NSERVICEBUS_AMAZONSQS_PROXY_AUTHENTICATION_PASSWORD` respectively. To achieve the same behavior the client factory has to be overriden like shown below.

snippet: 3to4_S3Proxy

### Native Deferral

The native deferral API has been deprecated. By default it is possible to send delays natively up to 15 min. For longer deferrals the unrestricted delayed delivery mechanism can be used:

snippet: 3to4_DelayedDelivery

Consult the delayed delivery documentation for more information.