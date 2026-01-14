---
title: NServiceBus DataBus feature
summary: How to handle messages that are too large to be sent by a transport natively by using NServiceBus DataBus
component: DataBus
reviewed: 2024-08-01
redirects:
 - nservicebus/databus
 - samples/pipeline/stream-properties
related:
 - samples/databus/file-share-databus
 - samples/databus/custom-serializer
 - samples/databus/blob-storage-databus
 - samples/databus/redis
---

Although messaging systems work best with small message sizes, some scenarios require sending binary large objects ([blobs](https://en.wikipedia.org/wiki/Binary_large_object)) data along with a message (also known as a [_Claim Check_](https://learn.microsoft.com/en-us/azure/architecture/patterns/claim-check)). For this purpose, NServiceBus has a `DataBus` feature to overcome the message size limitations imposed by the underlying transport.

partial: obsolete

## How it works

Instead of serializing the payload along with the rest of the message, the `DataBus` approach involves storing the payload in a separate location that both the sending and receiving parties can access, then putting the reference to that location in the message.

If the location is not available upon sending, the send operation will fail. When a message is received and the payload location is not available, the receive operation will fail too, resulting in the standard NServiceBus retry behavior, possibly resulting in the message being moved to the error queue if the error could not be resolved.

## Transport message size limits

The `DataBus` may be used to send messages which exceed the transport's message size limit, which is determined by the message size limit of the underlying queuing/storage technologies.

> [!NOTE]
> Not all transports have message size limits and some technologies, such as Azure Service Bus or Amazon SQS, have increased over time. Current message size limits are stated in the documentation linked in the table below.

| Transport                         | Maximum size |
| --------------------------------- | ------------:|
| Amazon SQS                        | [1MB for SQS](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/quotas-messages.html) and [256KB for SNS](https://docs.aws.amazon.com/general/latest/gr/sns.html) |
| Amazon SQS (using S3)             | [2GB](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/quotas-messages.html) |
| Azure Storage Queues              | [64KB](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-azure-and-service-bus-queues-compared-contrasted#capacity-and-quotas) |
| Azure Service Bus (Standard tier) | [256KB](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-azure-and-service-bus-queues-compared-contrasted#capacity-and-quotas) |
| Azure Service Bus (Premium tier)  | [100MB](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-premium-messaging#large-messages-support) |
| RabbitMQ                          | Configured by [`max_message_size`](https://www.rabbitmq.com/configure.html#config-items) |
| SQL Server                        | No limit |
| Learning                          | No limit |
| MSMQ                              | [4MB](https://learn.microsoft.com/en-us/previous-versions/windows/desktop/msmq/ms711436(v=vs.85)#maximum-message-size) |

## Enabling the `DataBus`

See the individual `DataBus` implementations for details on enabling and configuring the `DataBus`.

- [File Share Data Bus](file-share.md)
- [Azure Blob Storage Data Bus](azure-blob-storage.md)

## Cleanup

By default, blobs are stored with no set expiration. If messages have a [time to be received](/nservicebus/messaging/discard-old-messages.md) set, the `DataBus` will pass this along to the data bus storage implementation.

> [!NOTE]
> The value used should be aligned with the [ServiceControl audit retention period](/servicecontrol/how-purge-expired-data.md) if it is required that `DataBus` blob keys in messages sent to the audit queue can still be fetched.

## Specifying `DataBus` properties

There are two ways to specify the message properties to be sent using the `DataBus`:

 1. Using the `ClaimCheckProperty<T>` type
 1. Message conventions

> [!NOTE]
> `DataBus` properties must be top-level properties on a message class.
>
> Apart from `byte[]`, any data type is supported as long as it is serializable. For example, you can use `string`, custom classes, or other serializable types as `DataBus` properties.

### Using `ClaimCheckProperty<T>`

Set the type of the property to be sent over as `ClaimCheckProperty<byte[]>`, or any other serializable type:

snippet: MessageWithLargePayload

### Using message conventions

NServiceBus also supports defining `DataBus` properties by convention. This allows data properties to be sent using the `DataBus` without using `ClaimCheckProperty<T>`, thus removing the need for having a dependency on NServiceBus from the message types.

In the configuration of the endpoint include:

snippet: DefineMessageWithLargePayloadUsingConvention

Set the type of the property as `byte[]`, or any other serializable type:

snippet: MessageWithLargePayloadUsingConvention

partial: serialization

## `DataBus` attachments cleanup

The `DataBus` implementations each behave differently when cleaning up the physical attachments used to transfer the data properties.

### Why attachments are not removed by default

Automatically removing these attachments can cause problems in many situations. For example:

- The supported implementations do not participate in distributed transactions. If the message handler throws an exception and the transaction rolls back, the delete operation on the attachment cannot be rolled back. Therefore, when the message is retried, the attachment will no longer be present, causing additional problems.
- The message can be deferred so that the file will be processed later. Removing the file after deferring the message, results in a message without the corresponding file.
- Functional requirements might dictate the message to be available for a longer duration.
- If the `DataBus` feature is used when publishing an event to multiple subscribers, neither the publisher nor any specific subscribing endpoint can determine when all subscribers have successfully processed the message, allowing the file to be cleaned up.
- If message processing fails, it will be handled by the [recoverability feature](/nservicebus/recoverability/). This message can then be retried some period after that failure. The attachment files need to exist for that message to be re-processed correctly.

## Alternatives

> [!NOTE]
> A combination of these techniques may be used.

- Use a different transport or different tier (e.g. Azure Service Bus _Premium_ instead of _Standard_).
- Use message body compression, which works well on text-based payloads like XML and JSON or any payload (text or binary) that contains repetitive data.
  - The [message mutator sample](/samples/messagemutators/) demonstrates message body compression.
- Use a more efficient serializer, such as a binary serializer.
  - A custom serializer can usually be implemented in only a few lines of code.
  - Some binary [serializers are maintained by the community](/nservicebus/community/#serializers).
- Use [NServiceBus.Attachments](/nservicebus/community/#nservicebus-attachments) for unbounded binary payloads. The package is similar to the `DataBus`, but has some differences:
  - Read on demand: Attachments are only retrieved when read by a consumer.
  - Async enumeration: The package supports processing all data items using an `IAsyncEnumerable`.
  - No serialization: The serializer is not used, which may result in a significant reduction in memory usage.
  - Direct stream access: This makes the package more suitable for [binary large objects (blobs](https://en.wikipedia.org/wiki/Binary_large_object) since stream contents do not necessarily have to be loaded into memory before storing them or when retrieving them.

## Other considerations

### Monitoring and reliability

The storage location for `DataBus` blobs is critical to the operation of endpoints. As such, it should be as reliable as other infrastructure, such as the transport or persistence. It should also be monitored for errors and be actively maintained. Since messages cannot be sent or received when the storage location is unavailable, it may be necessary to stop endpoints when maintenance tasks occur.

### Auditing

The data stored in `DataBus` blobs may be considered part of an audit record. In these cases, the blobs should be archived alongside messages for as long as the audit record is required.
