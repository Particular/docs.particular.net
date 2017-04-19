---
title: DataBus
component: Core
reviewed: 2016-08-29
tags:
 - DataBus
redirects:
 - nservicebus/databus
related:
 - samples/file-share-databus
 - samples/azure/blob-storage-databus
---

Messages are intended to be small. Some scenarios require sending large binary data along with a message. For this purpose, NServiceBus has a DataBus feature to overcome the message size limitations imposed by underlying transport.


## How it works

The `DataBus` approach is to store a large binary payload in a location that both the sending and receiving parties can access. The message is sent with a reference to the location, and upon processing, the payload is brought, allowing the receiving part to access the message along with the payload. If the location is not available upon sending, the message fails the send operation. When the payload location is not available, the receive operation fails as well and results in standard NServiceBus behavior, causing retries and eventually going into the error queue.

The [Handling large stream properties via pipeline](/samples/pipeline/stream-properties/) sample demonstrates a purely stream-based approach implemented by leveraging NServiceBus pipeline. 


## Enabling the DataBus

See the individual DataBus implementations for details on enabling and configuring the DataBus.

 * [FileShare DataBus](file-share.md)
 * [Azure Blob Storage DataBus](azure-blob-storage.md)
 * [MongoDB DataBus](/nservicebus/messaging/databus/mongodb-tekmaven.md)


## Specifying DataBus properties

There are two ways to specify the message properties to be sent using DataBus

 1. Using `DataBusProperty<T>` type
 1. Message conventions

Note: The properties must be of type `byte[]`.


### Using DataBusProperty<T>

Properties defined using the `DataBusProperty<T>` type are not treated as part of a message, but persist externally based on the type of `DataBus` used, and are linked to the original message using a unique key.

snippet: MessageWithLargePayload


### Using message conventions

NServiceBus supports defining DataBus properties via convention. This allows defining a convention for data properties to be sent using `DataBus` without using `DataBusProperty<T>`.

snippet: DefineMessageWithLargePayloadUsingConvention

snippet: MessageWithLargePayloadUsingConvention


## DataBus attachments cleanup

DataBus implementations currently behaves differently with regard to cleanup of physical attachments used to transfer data properties depending on the implementation used.


### Reasons why attachments are not removed by default

Automatically removing these attachments can cause problems in many situations. For example:

 * The supported data bus implementations do not participate in distributed transactions. If for some reason, the message handler throws an exception and the transaction rolls back, the delete operation on the attachment cannot be rolled back. Therefore, when the message is retried, the attachment will no longer be present causing additional problems.
 * The message can be deferred so that the file will be processed later. Removing the file after deferring the message, results in a message without the corresponding file.
 * Functional requirements might dictate the message to be available for a longer duration.
 * If the DataBus feature is used in combination with multiple subscribers, the subscribers cannot determine who should remove the file.
 * If a messages fails it will be handled by [recoverability](/nservicebus/recoverability/). This message can then be retried some period after that failure. The databus files need to exist for that message to be re-processed correctly.


## Other Considerations


### Monitoring and reliability

The storage location for DataBus blobs is critical to the operation of Endpoints. As such is should be as reliable as other dependencies such as the Transport or Persistence. It should also be monitored for error/problems and be actively maintained. Since messages cannot be sent or received when storage location is unavailable, it may be necessary to stop Endpoints when maintenance tasks occur.


### Auditing

The data stored in DataBus blobs may be considered part of an Audit record. In these cases DataBus blobs should be archived alongside messages in for as long as the Audit record is required. 
