---
title: Configuration Options
summary: Configuration options for the SQS transport.
component: SQS
reviewed: 2026-06-29
redirects:
- nservicebus/sqs/configuration-options
---

> [!NOTE]
> The transport does not support `transport.ConnectionString(...)` to specify the connection string via code.

partial: source

partial: clientfactory

partial: donotwrapoutgoingmessages

## Retention period

**Optional**

**Default**: 4 days

This is the maximum time that a message will be retained within SQS and S3. If a sent message is not received and successfully processed within the specified time, it will be lost. This value applies to both SQS and S3: messages in SQS will be deleted after this period, and large message bodies stored in S3 will be automatically deleted after this period.

The maximum value is 14 days.

**Example**: To set this to the maximum value, specify:

snippet: MaxTTL

> [!NOTE]
> [Large message payloads stored in S3](topology.md#s3) are never deleted by the receiving endpoint, regardless of whether they were successfully handled. The S3 aging policy controls payload deletion and respects the configured TTL. Since message payloads stored in S3 are important for audited and failed messages in ServiceControl, it is crucial that the [ServiceControl message retention period](/servicecontrol/how-purge-expired-data.md) aligns with the configured SQS and S3 TTLs.

## Queue name prefix

**Optional**

**Default**: None

This string value is prepended to the name of every SQS queue referenced by the endpoint. This is useful when deploying multiple instances of the same application in the same AWS region (e.g., development, QA, and production), and when the queue names must be distinguished from one another.

**Example**: For a development instance, specify:

snippet: QueueNamePrefix

For example, queue names for the endpoint called "SampleEndpoint" might be:

```
DEV-SampleEndpoint
DEV-SampleEndpoint-Retries
DEV-SampleEndpoint-Timeouts
DEV-SampleEndpoint-TimeoutsDispatcher
```

partial: queuenamegenerator

## Offload large messages to S3

**Optional**

**Default**: Disabled. Any attempt to send a message larger than the SQS limit will fail.

partial: s3-offload-size

If the specified bucket doesn't exist, it will be created when the endpoint starts.

**Example**: To use a bucket named `nsb-sqs-messages`, specify:

snippet: S3BucketForLargeMessages

### Key prefix

**Mandatory**

This is the path in the specified S3 bucket for storing large messages.

### S3 Client

**Optional**

**Default**: `new AmazonS3Client()`

By default, the transport uses a parameterless constructor to build the S3 client. This overrides the default S3 client with a custom one.

**Example**: To use a custom client, specify:

snippet: S3ClientFactory

#if-version [6.1,)
> [!NOTE]
> If a custom S3 client is provided, it will not be disposed of when the endpoint is stopped.
#end-if

### Encryption

**Optional**

**Default**: Disabled

Specifies how large messages stored in S3 are encrypted. The default option is no encryption. The alternative is to use a managed encryption key:

snippet: S3ServerSideEncryption

or to provide a custom key:

snippet: S3ServerSideCustomerEncryption

partial: payload-signing

partial: visibility-timeout

partial: v1compatibilitymode

partial: delayeddelivery

partial: topics

partial: policy

partial: message-driven-pubsub-compatibility-mode

partial: message-size-calculation-reserved-bytes
