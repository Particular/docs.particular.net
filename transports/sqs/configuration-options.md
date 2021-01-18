---
title: Configuration Options
summary: Configuration options for the SQS transport.
component: SQS
reviewed: 2019-10-17
redirects:
- nservicebus/sqs/configuration-options
---

partial: connectionstringsupport

partial: maxReceiveMessageBatchSize

partial: source

partial: clientfactory

## MaxTTLDays

**Optional**

**Default**: 4

This is the maximum number of days that a message will be retained within SQS and S3. When a sent message is not received and successfully processed within the specified time, the message will be lost. This value applies to both SQS and S3 - messages in SQS will be deleted after this amount of time, and large message bodies stored in S3 will automatically be deleted after this amount of time.

The maximum value is 14 days.

**Example**: To set this to the maximum value, specify:

snippet: MaxTTL

NOTE: [Large message payloads stored in S3](topology.md#s3) are never deleted by the receiving endpoint, regardless of whether they were successfully handled. The S3 aging policy controls the deletion of the payload and will respect the configured TTL. Since message payloads stored in S3 are important for audited and failed messages stored in ServiceControl, it is crucial that the [ServiceControl message retention period](/servicecontrol/how-purge-expired-data.md) is aligned with the configured SQS and S3 TTL.

## QueueNamePrefix

**Optional**

**Default**: None

This string value is prepended to the name of every SQS queue referenced by the endpoint. This is useful when deploying many instances of the same application in the same AWS region (e.g. a development instance, a QA instance, and a production instance), and the queue names must be distinguished from each other.

**Example**: For a development instance, specify:

snippet: QueueNamePrefix

For example, queue names for the endpoint called "SampleEndpoint" might be:

```
DEV-SampleEndpoint
DEV-SampleEndpoint-Retries
DEV-SampleEndpoint-Timeouts
DEV-SampleEndpoint-TimeoutsDispatcher
```

## S3BucketForLargeMessages

**Optional**

**Default**: Empty. Any attempt to send a large message with an empty S3 bucket will fail.

This is the name of an S3 Bucket that will be used to store message bodies for messages larger than 256kB in size. If this option is not specified, S3 will not be used at all. Any attempt to send a message larger than 256kB will throw an exception if a value is not supplied for this parameter.

If the specified bucket doesn't exist, it will be created when the endpoint starts.

**Example**: To use a bucket named `nsb-sqs-messages`, specify:

snippet: S3BucketForLargeMessages


### S3KeyPrefix

**Optional**

**Default**: Empty

This is the path within the specified S3 Bucket to store large message bodies. If this option is specified without a value for S3BucketForLargeMessages, an exception will be thrown.

partial: s3clientfactory

partial: v1compatibilitymode

partial: delayeddelivery

partial: topics

partial: policy

partial: message-driven-pubsub-compatibility-mode