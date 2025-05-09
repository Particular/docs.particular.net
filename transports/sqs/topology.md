---
title: Topology
summary: Identify the physical components used by the Amazon SQS transport and how they interact
component: SQS
reviewed: 2025-04-08
---

The topology used by the transport is composed of several AWS components.

partial: pub-sub-diagram

## SQS

Amazon SQS exposes queues via [service endpoints](https://docs.aws.amazon.com/general/latest/gr/sqs-service.html#sqs_region) that are publicly available via [HTTPS](https://en.wikipedia.org/wiki/HTTPS). An [NServiceBus endpoints](/nservicebus/concepts/glossary.md#endpoint) can access SQS Queues whether they are deployed in AWS or not; as long as the endpoint can reach both SQS and S3 via HTTPS it can use the transport. By default, NServiceBus endpoints [process messages from an SQS queue with the same name](/nservicebus/endpoints/specify-endpoint-name.md#input-queue) as the endpoint.

The transport initiates all network connections to SQS and S3; hence the endpoint itself does not need to be publicly accessible and can reside behind a firewall or proxy.

The transport uses SQS [Standard Queues](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/standard-queues.html).

SQS queues support [competing consumers](https://www.enterpriseintegrationpatterns.com/patterns/messaging/CompetingConsumers.html). When an endpoint scales out to multiple instances, each instance consumes messages from the same input queue.

partial: pub-sub

partial: pub-sub-hybrid

## S3

[SQS supports a maximum message size of 256 KiB](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/quotas-messages.html). The transport works around this size limit by using [Amazon S3](https://docs.aws.amazon.com/AmazonS3/latest/dev/Welcome.html) to store message payloads for messages that are larger than 256 KiB. This allows the transport to send and receive messages of practically any size. Note that messages that fit within the size limit only use SQS; S3 does not come into play.

When a large message is sent, the transport uploads the message body to an S3 bucket and then sends an SQS message that contains a reference to the S3 object. On the receiving end, the transport receives the message from SQS, identifies the reference to the S3 object, downloads it, and processes the message as usual. When the message is to be deleted, the transport deletes the message from SQS and then deletes it from S3. To ensure that the message is deleted from S3, the transport applies a [lifecycle policy](https://docs.aws.amazon.com/AmazonS3/latest/dev/object-lifecycle-mgmt.html) to the S3 bucket that deletes any messages that are older than the configured [retention period](/transports/sqs/configuration-options.md#retention-period).
