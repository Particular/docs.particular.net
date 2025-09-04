---
title: Amazon SQS Transport
summary: A transport for Amazon Web Services Simple Queue Service.
component: SQS
reviewed: 2024-07-11
related:
 - samples/aws/sqs-simple
redirects:
- nservicebus/sqs/index
- transports/sqs/why-use-sqs
---

[Simple Queue Service (SQS)](https://aws.amazon.com/sqs/) is a message queue service provided by [Amazon Web Services](https://aws.amazon.com/).

## Transport at a glance

|Feature                    |   |
|:---                       |---
|Transactions |None, ReceiveOnly (Message visibility timeout)
|Pub/Sub                    |Native (Requires SNS, supports hybrid-mode for migration purposes)
|Timeouts                   |Native (Requires FIFO Queues)
|Large message bodies       |Native (Requires S3)
|Scale-out             |Competing consumer
|Scripted Deployment        |Built-in CLI, C#
|Installers                 |Optional
|Native integration         |[Supported](native-integration.md)
|Case Sensitive             |Yes
|Local development          |[Supported via LocalStack](/nservicebus/aws/local-development.md)

## Advantages

* Fully managed turn-key messaging infrastructure. SQS queues requires little effort to set up, maintain, and manage over time.
* Integrates seamlessly with other services provided by AWS, such as [IAM](https://docs.aws.amazon.com/iam/index.html), [CloudWatch](https://aws.amazon.com/cloudwatch/), and [Lambda](https://aws.amazon.com/lambda/). For organizations already committed to AWS, SQS is a natural choice.
* Can be used as a gateway between endpoints that may not have direct connectivity to each other.
* Can send and receive large messages that exceed the queue limitations by storing large payloads in S3. For more information review the documentation for the transport [topology](topology.md#s3) and [configuration options](configuration-options.md).

## Disadvantages

* Like other message brokers, there is no local store-and-forward mechanism available. If an endpoint cannot reach SQS, either due to network problems or if SQS is unavailable, the endpoint will not be able to send nor receive messages.
* Can be expensive with large volumes of messages.

## Prerequisites

An [AWS IAM](https://docs.aws.amazon.com/IAM/latest/UserGuide/introduction.html) account with a pair of [Access Keys](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-getting-started.html) is required.

The IAM account requires the following permissions to provision infrastructure and run the transport:

#### [SQS permissions](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-api-permissions-reference.html)

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

partial: sns-permissions

#### [S3 permissions](https://docs.aws.amazon.com/AmazonS3/latest/dev/using-with-s3-actions.html)

* CreateBucket
* DeleteObject
* GetObject
* PutObject
* PutLifecycleConfiguration
* GetLifecycleConfiguration
* ListAllMyBuckets

#### Other permissions

* If using server-side encryption of SQS queues, all NServiceBus endpoints (as well as [ServiceControl](/servicecontrol)) will require the `kms:GenerateDataKey` permission in order to support [key management](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-key-management.html).

## Configuration

partial: basic-config

## Retries and timeouts

The SQS transport uses the default [retry and timeout](https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/retries-timeouts.html) values implemented by the [AWS SDK for .NET](https://aws.amazon.com/sdk-for-net/):

| Parameter          | Default value |
|--------------------|---------------|
| `MaxErrorRetries`  | 4             |
| `RequestTimeout`   | 100s          |
| `ReadWriteTimeout` | 300s          |

> [!NOTE]
> NServiceBus will perform [immediate](/nservicebus/recoverability/#immediate-retries) and [delayed](/nservicebus/recoverability/#delayed-retries) retries in addition to retries performed internally by the SQS client.

## Batching

Messages sent from within a handler are [batched](/nservicebus/messaging/batched-dispatch.md) with up to ten messages per batch depending on the size of the message. Messages sent outside a handler are not batched.
