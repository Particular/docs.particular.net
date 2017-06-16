---
title: Amazon SQS Transport
summary: A transport for Amazon Web Services Simple Queue Service.
component: SQS
tags:
- AWS
---

[Simple Queue Service (SQS)](https://aws.amazon.com/sqs/) is a message queue service provided by [Amazon Web Services](https://aws.amazon.com/).


## Advantages and Disadvantages 


### Advantages 

 * Fully managed turn-key messaging infrastructure. SQS queues requires very little effort to set up, maintain and manage over time.
 * Integrates seamlessly with other services provided by AWS, for example, [IAM](https://aws.amazon.com/documentation/iam/), [CloudWatch](https://aws.amazon.com/cloudwatch/), [Lambda](https://aws.amazon.com/lambda/), etc. For organizations already committed to AWS, SQS is a natural choice.
 * Can be used as a gateway between endpoints that may not have direct connectivity to each-other.


### Disadvantages 

 * Like other message brokers, there is no local store-and-forward mechanism available. If an endpoint cannot reach SQS, either due to network problems or if SQS is unavailable, the endpoint will not be able to send nor receive messages.
 * Can be relatively expensive when using larger volumes of messages.


## Getting Started


### Set Up An AWS Account

To use NServiceBus.AmazonSQS, an [AWS IAM](http://docs.aws.amazon.com/IAM/latest/UserGuide/IAM_Introduction.html) account with a pair of [Access Keys](http://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSGettingStartedGuide/AWSCredentials.html) is required.

The IAM account requires the following [SQS permissions](http://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-api-permissions-reference.html) and [S3 permissions](http://docs.aws.amazon.com/AmazonS3/latest/dev/using-with-s3-actions.html):

 * SQS::CreateQueue
 * SQS::DeleteMessage
 * SQS::GetQueueUrl
 * SQS::ReceiveMessage
 * SQS::SendMessage
 * SQS::SetQueueAttributes
 * SQS::ChangeMessageVisibility
 * S3::PutBucket
 * S3::DeleteObject
 * S3::GetObject
 * S3::PutObject
 * S3::PutLifecycleConfiguration
 * S3::ListAllMyBuckets

By default, NServiceBus.AmazonSQS will look for the Access Key ID and Secret Access Key in system environment variables of the machine that is running the endpoint:

 * Access Key ID goes in `AWS_ACCESS_KEY_ID`
 * Secret Access Key goes in `AWS_SECRET_ACCESS_KEY`

snippet: SqsTransport