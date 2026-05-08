---
title: Transaction Support
summary: Explore the transaction modes the transport supports.
component: SQS
reviewed: 2026-02-17
---

The Amazon SQS transport supports the following [Transport Transaction Modes](/transports/transactions.md):

 * Transport transaction - Receive Only
 * Unreliable (Transactions Disabled)

The transport functions similarly regardless of whether it runs in Receive Only or Unreliable mode.

As the transport receives messages from the SQS queue for processing, SQS hides the received messages from other consumers for a configurable [visibility timeout](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-visibility-timeout.html) period. The transport automatically renews the message visibility timeout during processing to prevent messages from being reprocessed while a handler is still executing. This is a best-effort operation that may fail due to clock skew, network interruptions, or SQS service throttling. For more details, see [Message visibility timeout](/transports/sqs/configuration-options.md#message-visibility).

After the endpoint has successfully processed a message, the transport deletes the message from the SQS queue. Messages that are not processed successfully will be either sent back to the input queue as part of deferred retries or moved to the error queue, depending on how the [recoverability](/nservicebus/recoverability) of the endpoint is configured.

> [!NOTE]
> The transport uses [Amazon SQS Standard Queues](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/standard-queues.html) which guarantee [at-least-once delivery](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/standard-queues-at-least-once-delivery.html) of messages. Furthermore, if the connection to Amazon SQS is lost for any reason before a message can be deleted, even if the message was successfully processed, the message will become available to other consumers when the [visibility timeout](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-visibility-timeout.html) expires. Therefore it is possible for endpoints to successfully process the same messages more than once. The [Outbox](/nservicebus/outbox) feature can be used to deduplicate incoming messages if required.
