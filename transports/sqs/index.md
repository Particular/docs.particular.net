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

When sending messages it can happen that your sending is throttled. When that happens the following exception is logged.
```
2017-11-14 23:10:24,314|ERROR|18|NServiceBus.Transports.SQS.MessageDispatcher|Exception from Send.
Amazon.SQS.AmazonSQSException: Request is throttled. ---> Amazon.Runtime.Internal.HttpErrorResponseException: The remote server returned an error: (403) Forbidden. ---> System.Net.WebException: The remote server returned an error: (403) Forbidden.
```

When this happens while receiving a message then this can safely be ignored as [recoverability features](/nservicebus/recoverability/) should resolve this. If you are sending messages outside of a receiving message context you will have to be be aware that your send can fail. Especially when sending messages in bulk, specifically in parallel, you can get throttled if the sender has enough resources to suddenly flood the Amazon SQS infrastructure.

This can be resolved by limiting the amount of concurrent send tasks. Send 100,000 messages of which, for example, max 100 messages concurrently by [using a Semaphore](/nservicebus/handlers/async-handlers.md#large-amount-of-concurrent-message-operations), usually you can still easily get into sending thousands of messages per second.


