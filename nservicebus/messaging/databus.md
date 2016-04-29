---
title: DataBus Feature
summary: DataBus feature
tags:
- DataBus
- Attachments
redirects:
- nservicebus/databus
related:
- samples/databus
- samples/azure/blob-storage-databus
---

## The DataBus feature

Messages are intended to be small. Some scenarios require sending large binary data along with a message. For this purpose, NServiceBus has a DataBus feature to overcome the message size limitations imposed by underlying transport.


## How it works

The `DataBus` approach is to store a large payload in a location that both the sending and receiving parties can access. The message is sent with a reference to the location, and upon processing, the payload is brought, allowing the receiving part to access the message along with the payload. If the location is not available upon sending, the message fails the send operation. When the payload location is not available, the receive operation fails as well and results in standard NServiceBus behavior, causing retries and eventually going into the error queue.

The FileShare DataBus also [leverage both serialization and headers](/nservicebus/messaging/headers.md#fileshare-databus-headers) to provide its functionality.


## Enabling the DataBus

NServiceBus supports two DataBus implementations:

 * `FileShareDataBus`
 * `AzureDataBus`

To enable DataBus, NServiceBus needs to be configured. For file share based DataBus:

snippet:FileShareDataBus

For Azure (storage blobs) based DataBus:

snippet:AzureDataBus

NOTE: The `AzureDataBus` implementation is part of the Azure transport package.


## Specifying DataBus properties

There are two ways to specify the message properties to be sent using DataBus

 1. Using `DataBusProperty<T>` type
 1. Message conventions


### Using DataBusProperty<T>

Properties defined using the `DataBusProperty<T>` type provided by NServiceBus are not treated as part of a message, but persist externally based on the type of `DataBus` used, and are linked to the original message using a unique key.

snippet:MessageWithLargePayload


### Using message conventions

NServiceBus supports defining DataBus properties via convention. This allows defining a convention for data properties to be sent using `DataBus` without using `DataBusProperty<T>`.

snippet:DefineMessageWithLargePayloadUsingConvention

snippet:MessageWithLargePayloadUsingConvention


## DataBus attachments cleanup

NServiceBus DataBus implementations currently behaves differently with regard to cleanup of physical attachments used to transfer data properties depending on the implementation used.


### Reasons why attachments are not removed by default

Automatically removing these attachments can cause problems in many situations. For example:

 * The supported data bus implementations do not participate in distributed transactions. If for some reason, the message handler throws an exception and the transaction rolls back, the delete operation on the attachment cannot be rolled back. Therefore, when the message is retried, the attachment will no longer be present causing additional problems.
 * The message can be deferred so that the file will be processed later. Removing the file after deferring the message, results in a message without the corresponding file.
 * Functional requirements might dictate the message to be available for a longer duration.
 * If the outbox feature in NServiceBus is enabled, the message will be removed from the incoming queue, but it might not have been processed yet.
 * If the DataBus feature is used in combination with multiple subscribers, the subscribers cannot determine who should remove the file.
 * If a messages fails it will be forwarded to the [error queue](/nservicebus/errors/). This message can then be retried some period after that failure. The databus files need to exist fot that message to be re-processed correctly.


### AzureDataBus Implementation

AzureDataBus will **remove** the Azure storage blobs used for physical attachments after the message is processed if the `TimeToBeReceived` value is specified. When this value isn't provided, the physical attachments will not be removed.


#### Cleanup Strategy for AzureDataBus

Specify a proper value for the `TimeToBeReceived` property. For more details on how to specify this, read this article on [discarding old messages](/nservicebus/messaging/discard-old-messages.md).


### FileShareDataBus

WARNING: FileShareDataBus **does not** remove physical attachments once the message has been processed.


#### Custom cleanup strategy for FileShareDataBus

The business requirements can indicate how a message and its corresponding file should be processed and when the files can safely be removed. One strategy to deal with these attachments is to set up a cleanup policy which removes any attachments after a certain number of days has passed based on business SLA.

The file location used by the databus is set during configuration time.

snippet:DefineFileLocationForDatabusFiles

This same location should be used when performing the cleanup.

So for example this path can be used in a Handler for a message containing databus properties.

snippet:HandlerThatCleansUpDatabus

 
## Configuring AzureDataBus

The following extension methods are available for changing the behavior of `AzureDataBus` defaults:

snippet:AzureDataBusSetup

 * `ConnectionString()`: the connection string to the storage account for storing DataBus properties, defaults to `UseDevelopmentStorage=true`
 * `Container()`: container name, defaults to '`databus`'
 * `BasePath()`: the blobs base path under the container, defaults to empty string
 * `DefaultTTL`: time in seconds to keep blob in storage before it is removed, defaults to `Int64.MaxValue` seconds
 * `MaxRetries`: number of upload/download retries, defaults to 5 retries
 * `NumberOfIOThreads`: number of blocks that will be simultaneously uploaded, defaults to 5 threads
 * `BackOffInterval`: the back-off time between retries, defaults to 30 seconds
 * `BlockSize`: the size of a single block for upload when the number of IO threads is more than 1, defaults to 4MB