---
title: Transaction Support
summary: Explore the transaction modes the transport supports.
component: SQS
reviewed: 2021-05-11
---

The Amazon SQS transport supports the following [Transport Transaction Modes](/transports/transactions.md):

 * Transport transaction - Receive Only
 * Unreliable (Transactions Disabled)

The transport functions in the same way regardless of whether it is running in Receive Only or Unreliable mode. 

As the transport receives messages from the SQS queue for processing, SQS hides the received messages from other consumers for a configurable [visibility timeout](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-visibility-timeout.html) period. After the endpoint has successfully processed a message, the transport deletes the message from the SQS queue. Messages that are not processed successfully will be either sent back to the input queue as part of deferred retries or moved to the error queue, depending on how the [recoverability](/nservicebus/recoverability) of the endpoint is configured.

WARNING: The transport uses [Amazon SQS Standard Queues](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/standard-queues.html) which guarantee [at-least-once delivery](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/standard-queues.html#standard-queues-at-least-once-delivery) of messages. Furthermore if the connection to Amazon SQS is lost for any reason before a message can be deleted, even if the message was successfully processed, the message will become available to other consumers when the [visibility timeout](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-visibility-timeout.html) expires. Therefore it is possible for endpoints to successfully process the same messages more than once. Consider using the [Outbox](/nservicebus/outbox) feature to deduplicate incoming messages.
