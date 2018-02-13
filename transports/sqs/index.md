---
title: Amazon SQS Transport
summary: A transport for Amazon Web Services Simple Queue Service.
component: SQS
reviewed: 2017-10-16
related:
 - samples/sqs/simple
tags:
- AWS
redirects:
- nservicebus/sqs/index
---

[Simple Queue Service (SQS)](https://aws.amazon.com/sqs/) is a message queue service provided by [Amazon Web Services](https://aws.amazon.com/).


## Advantages

 * Fully managed turn-key messaging infrastructure. SQS queues requires very little effort to set up, maintain and manage over time.
 * Integrates seamlessly with other services provided by AWS, for example, [IAM](https://aws.amazon.com/documentation/iam/), [CloudWatch](https://aws.amazon.com/cloudwatch/), [Lambda](https://aws.amazon.com/lambda/), etc. For organizations already committed to AWS, SQS is a natural choice.
 * Can be used as a gateway between endpoints that may not have direct connectivity to each-other.


## Disadvantages

 * Like other message brokers, there is no local store-and-forward mechanism available. If an endpoint cannot reach SQS, either due to network problems or if SQS is unavailable, the endpoint will not be able to send nor receive messages.
 * Can be relatively expensive when using larger volumes of messages.


## Getting Started

An [AWS IAM](http://docs.aws.amazon.com/IAM/latest/UserGuide/introduction.html) account with a pair of [Access Keys](http://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-getting-started.html) is required.

The IAM account requires the following:


#### [SQS permissions](http://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-api-permissions-reference.html)

 * CreateQueue
 * DeleteMessage
 * DeleteMessageBatch
 * GetQueueUrl
 * ReceiveMessage
 * SendMessage
 * SendMessageBatch
 * SetQueueAttributes
 * ChangeMessageVisibility
 * ChangeMessageVisibilityBatch
 * PurgeQueue


#### [S3 permissions](http://docs.aws.amazon.com/AmazonS3/latest/dev/using-with-s3-actions.html)

 * CreateBucket
 * DeleteObject
 * GetObject
 * PutObject
 * PutLifecycleConfiguration
 * GetLifecycleConfiguration
 * ListAllMyBuckets

partial: credentials


## Retries and Timeouts


The SQS transport uses the default [retries and timeouts](http://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/retries-timeouts.html) values implemented by the [AWS SDK for .NET](https://aws.amazon.com/sdk-for-net/):

| Parameter          | Default value |
|--------------------|---------------|
| `MaxErrorRetries`  | 4             |
| `RequestTimeout`   | 100s          |
| `ReadWriteTimeout` | 300s          |

NOTE: NServiceBus will perform [immediate](/nservicebus/recoverability/#immediate-retries) and [delayed](/nservicebus/recoverability/#delayed-retries) retries in addition to retries performed internally by the SQS client.


## Troubleshooting

### AmazonSQSException: Request is throttled

Amazon SQS could apply throttling, it can deal with a very large continious throughput but if there are sudden spikes the service can apply throttling.

When throttling happens the following exception is logged:

```
2017-11-14 23:10:24,314|ERROR|18|NServiceBus.Transports.SQS.MessageDispatcher|Exception from Send.
Amazon.SQS.AmazonSQSException: Request is throttled. ---> Amazon.Runtime.Internal.HttpErrorResponseException: The remote server returned an error: (403) Forbidden. ---> System.Net.WebException: The remote server returned an error: (403) Forbidden.
```

Throttling is more likely to happen when sending a large number messages concurrently (in parallel). For example when using async/await, using a list of tasks to be executed with `Task.WaitAll()` could cause throttling and partial message delivery if any of the sends result in an thottling error.

To avoid possible Amazon throttling errors, limit the maximum number of concurrent sends. For example, allow only a small amount of messages to be sent concurrently as outlined in the [sending large amount of messages](/nservicebus/handlers/async-handlers.md#concurrency-large-amount-of-concurrent-message-operations) guidelines or don't use concurrent sends at all and sequentially send your messages.

Throttling can happen during any send or receive operation and can happen during the following scenarios:

- Incoming message (receiving)
- Sending from within a handler
- Sending outside of a handler


#### Incoming message (receiving)

For incoming messages throttling errors can be safely ignored as the message pump will just try to fetch the next available message again.

#### Sending from within a handler

The [recoverability feature](/nservicebus/recoverability/) should resolve it as any message sends and fails results in an exception which will be processed by the recoverability logic. If an incoming message would continiously result in a throttling error then the message will eventually be moved to the error queue.

A throttling error could result in ghost messages. Ghost messages are messages send even though the handler failed due to not being able to transmit all messages to the transport succesfully. This makes a throttling error similar to any other error could and can occur both with the default [batched message dispatch](/nservicebus/messaging/batched-dispatch) or when using [immediate dispatch](nservicebus/messaging/send-a-message#dispatching-a-message-immediately).

#### Sending outside of a handler

As message sending does not happen within a handler any failures during sending will not rely on the [recoverability feature](/nservicebus/recoverability/). Any retry logic must be manually implemented. If a throttling exception occurs 1 or more message are likely not transmitted to the transport if these is no custom error logic.




