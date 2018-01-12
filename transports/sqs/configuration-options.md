---
title: Configuration Options
summary: Configuration options for the SQS transport.
component: SQS
reviewed: 2017-06-28
tags:
- AWS
redirects:
- nservicebus/sqs/configuration-options
---

partial: connectionstringsupport

partial: maxReceiveMessageBatchSize

partial: source

partial: clientfactory

## MaxTTLDays

**Optional**

**Default**: 4.

This is the maximum number of days that a message will be retained within SQS and S3. When a sent message is not received and successfully processed within the specified time, the message will be lost. This value applies to both SQS and S3 - messages in SQS will be deleted after this amount of time expires, and large message bodies stored in S3 will automatically be deleted after this amount of time expires.

The maximum value is 14 days.

**Example**: To set this to the maximum value, specify:

snippet: MaxTTL


## QueueNamePrefix

**Optional**

**Default**: None.

This string value will be prepended to the name of every SQS queue referenced by the endpoint. This is useful when deploying many instances of the same application in the same AWS region (e.g. a development instance, a QA instance and a production instance), and the queue names need to be distinguished somehow.

**Example**: For a development instance, specify:

snippet: QueueNamePrefix

Queue names for the endpoint called "SampleEndpoint" might then look like:

```
DEV-SampleEndpoint
DEV-SampleEndpoint-Retries
DEV-SampleEndpoint-Timeouts
DEV-SampleEndpoint-TimeoutsDispatcher
```

## S3BucketForLargeMessages

**Optional**

**Default**: Empty. Any attempt to send a large message with an empty S3 bucket will fail.

This is the name of an S3 Bucket that will be used to store message bodies for messages that are larger than 256kb in size. If this option is not specified, S3 will not be used at all. Any attempt to send a message larger than 256kb will throw if this option hasn't been specified.

If the specified bucket doesn't exist, it will be created at endpoint start up.

**Example**: To use a bucket named `nsb-sqs-messages`, specify:

snippet: S3BucketForLargeMessages


### S3KeyPrefix

**Optional**

**Default**: Empty.

This is the path within the specified S3 Bucket to store large message bodies. It is an error to specify this option without specifying a value for S3BucketForLargeMessages.

partial: s3clientfactory

partial: nativeDeferral