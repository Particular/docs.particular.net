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

## What is the DataBus feature for

Messages are intended to be small. Some scenarios require sending large binary data along with a message. For this purpose, NServiceBus has a DataBus feature to allow you to overcome the message size limitations imposed by underlying transport.


## How it works

The `DataBus` approach is to store a large payload in a location that both the sending and receiving parties can access. The message is sent with a reference to the location, and upon processing, the payload is brought, allowing the receiving part to access the message along with the payload. If the location is not available upon sending, the message fails the send operation. When the payload location is not available, the receive operation fails as well and results in standard NServiceBus behavior, causing retries and eventually going into the error queue.

The FileShare DataBus also [leverage both serialization and headers](/nservicebus/messaging/headers.md#fileshare-databus-headers) to provide its functionality.


## How to enable DataBus

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
2. Message conventions


### Using DataBusProperty<T>

Properties defined using the `DataBusProperty<T>` type provided by NServiceBus are not treated as part of a message, but persist externally based on the type of `DataBus` used, and are linked to the original message using a unique key.

snippet:MessageWithLargePayload


### Using message conventions

NServiceBus supports defining DataBus properties via convention. This allows defining a convention for data properties to be sent using `DataBus` without using `DataBusProperty<T>`.

snippet:DefineMessageWithLargePayloadUsingConvention

snippet:MessageWithLargePayloadUsingConvention


## DataBus attachments cleanup

NServiceBus DataBus implementations currently behave differently with regard to cleanup of physical attachments used to transfer data properties. `FileShareDataBus` **does not** remove physical attachments once the message is gone. `AzureDataBus` **does** remove Azure storage blobs used for physical attachments once the message is gone.

### Custom cleanup strategy

NServiceBus cannot tell when a file should be deleted. Perhaps the message is (functionally) deferred for the file to be processed later, or it is passed along to another handler. When the file is removed upon committing the transaction, the deferred or passed on message cannot process the file anymore. Removing the file depends on your functional needs.

Looking at it from another perspective, the file system also isn’t transactional. Transactions spawned by NServiceBus might fail, but the file system cannot participate in them, resulting in a deleted file even when the transaction is aborted. Or if the outbox feature in NServiceBus is enabled, the message will be removed from the queuing storage, but it has not actually been processed yet. 

There are a large number of scenarios where removing the file can cause a problem. The only person who actually can tell when which file could be removed, is the developer, who should come up with a strategy to remove files. An option is a strategy like removing the file after (Max(SLA) + x-days), so that it is unlikely that the file still hasn’t been processed.

To obtain the file location use the

snippet:DefineFileLocationForDatabusFiles

snippet:FileLocationForDatabusFiles
 
## Configuring AzureDataBus

The following extension methods are available for changing the behavior of `AzureDataBus` defaults:

snippet:AzureDataBusSetup

 * `ConnectionString()`: the connection string to the storage account for storing DataBus properties, defaults to `UseDevelopmentStorage=true`
 * `Container()`: container name, defaults to '`databus`'
 * `BasePath()`: the blobs base path under the container, defaults to empty string
 * `DefaultTTL`: time in seconds to keep blob in storage before it is removed, defaults to `Int64.MaxValue` seconds
 * `MaxRetries`: number of upload/download retries, defaults to 5 retries
 * `NumberOfIOThreads`: number of blocks that will be simultaneously uploaded, defaults to 5 threads
 * `BackOffInterval`:  the back-off time between retries, defaults to 30 seconds
 * `BlockSize`: the size of a single block for upload when the number of IO threads is more than 1, defaults to 4MB


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

## What is the DataBus feature for

Messages are intended to be small. Some scenarios require sending large binary data along with a message. For this purpose, NServiceBus has a DataBus feature to allow you to overcome the message size limitations imposed by underlying transport.


## How it works

The `DataBus` approach is to store a large payload in a location that both the sending and receiving parties can access. The message is sent with a reference to the location, and upon processing, the payload is brought, allowing the receiving part to access the message along with the payload. If the location is not available upon sending, the message fails the send operation. When the payload location is not available, the receive operation fails as well and results in standard NServiceBus behavior, causing retries and eventually going into the error queue.

The FileShare DataBus also [leverage both serialization and headers](/nservicebus/messaging/headers.md#fileshare-databus-headers) to provide its functionality.


## How to enable DataBus

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
2. Message conventions


### Using DataBusProperty<T>

Properties defined using the `DataBusProperty<T>` type provided by NServiceBus are not treated as part of a message, but persist externally based on the type of `DataBus` used, and are linked to the original message using a unique key.

snippet:MessageWithLargePayload


### Using message conventions

NServiceBus supports defining DataBus properties via convention. This allows defining a convention for data properties to be sent using `DataBus` without using `DataBusProperty<T>`.

snippet:DefineMessageWithLargePayloadUsingConvention

snippet:MessageWithLargePayloadUsingConvention


## DataBus attachments cleanup

NServiceBus DataBus implementations currently behave differently with regard to cleanup of physical attachments used to transfer data properties. `FileShareDataBus` **does not** remove physical attachments once the message is gone. `AzureDataBus` **does** remove Azure storage blobs used for physical attachments once the message is gone.

### Custom cleanup strategy

NServiceBus cannot tell when a file should be deleted, as removing the file depends on your functional needs. There are a large number of scenarios where removing the file can cause a problem. 

- The message can be (functionally) deferred so that the file will be processed later, or it is passed along to another handler. When the file is removed upon committing the transaction, the deferred or passed on message cannot process the file anymore.
- The file system isn’t transactional. Transactions spawned by NServiceBus might fail, but the file system cannot participate in them, resulting in a deleted file even when the transaction is aborted.
- If the outbox feature in NServiceBus is enabled, the message will be removed from the incoming queue, but it might not actually have been processed yet. 

The business requirements should specify when the file should be processed and when it can be removed. Therefor the business should come up with a strategy to remove the files. An option is a strategy like removing the file after (Max(SLA) + x-days), so that it is unlikely that the file still hasn’t been processed.

To obtain the file location use the base path set up during confiration and the incoming DataBus properties

snippet:DefineFileLocationForDatabusFiles

snippet:FileLocationForDatabusFiles
 
## Configuring AzureDataBus

The following extension methods are available for changing the behavior of `AzureDataBus` defaults:

snippet:AzureDataBusSetup

 * `ConnectionString()`: the connection string to the storage account for storing DataBus properties, defaults to `UseDevelopmentStorage=true`
 * `Container()`: container name, defaults to '`databus`'
 * `BasePath()`: the blobs base path under the container, defaults to empty string
 * `DefaultTTL`: time in seconds to keep blob in storage before it is removed, defaults to `Int64.MaxValue` seconds
 * `MaxRetries`: number of upload/download retries, defaults to 5 retries
 * `NumberOfIOThreads`: number of blocks that will be simultaneously uploaded, defaults to 5 threads
 * `BackOffInterval`:  the back-off time between retries, defaults to 30 seconds
 * `BlockSize`: the size of a single block for upload when the number of IO threads is more than 1, defaults to 4MB


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

## What is the DataBus feature for

Messages are intended to be small. Some scenarios require sending large binary data along with a message. For this purpose, NServiceBus has a DataBus feature to allow you to overcome the message size limitations imposed by underlying transport.


## How it works

The `DataBus` approach is to store a large payload in a location that both the sending and receiving parties can access. The message is sent with a reference to the location, and upon processing, the payload is brought, allowing the receiving part to access the message along with the payload. If the location is not available upon sending, the message fails the send operation. When the payload location is not available, the receive operation fails as well and results in standard NServiceBus behavior, causing retries and eventually going into the error queue.

The FileShare DataBus also [leverage both serialization and headers](/nservicebus/messaging/headers.md#fileshare-databus-headers) to provide its functionality.


## How to enable DataBus

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
2. Message conventions


### Using DataBusProperty<T>

Properties defined using the `DataBusProperty<T>` type provided by NServiceBus are not treated as part of a message, but persist externally based on the type of `DataBus` used, and are linked to the original message using a unique key.

snippet:MessageWithLargePayload


### Using message conventions

NServiceBus supports defining DataBus properties via convention. This allows defining a convention for data properties to be sent using `DataBus` without using `DataBusProperty<T>`.

snippet:DefineMessageWithLargePayloadUsingConvention

snippet:MessageWithLargePayloadUsingConvention


## DataBus attachments cleanup

NServiceBus DataBus implementations currently behave differently with regard to cleanup of physical attachments used to transfer data properties. `FileShareDataBus` **does not** remove physical attachments once the message is gone. `AzureDataBus` **does** remove Azure storage blobs used for physical attachments once the message is gone.

### Custom cleanup strategy

Theoretically NServiceBus cannot tell when a file should be deleted. Perhaps the message is (functionally) deferred for the file to be processed later, or it is passed along to another handler. When the file is removed upon committing the transaction, the deferred or passed on message cannot process the file anymore. Removing the file depends on your functional needs.

Looking at it from another perspective, the file system also isn't transactional. Transactions spawned by NServiceBus might fail, but the file system cannot participate in them, resulting in a deleted file even when the transaction is aborted. Or if the outbox feature in NServiceBus is enabled, the message will be removed from the queuing storage, but it has not actually been processed yet. 

There are a large number of scenarios where removing the file can cause a problem. The only person who actually can tell when which file could be removed, is the developer, who should come up with a strategy to remove files. An option is a strategy like removing the file after (Max(SLA) + x-days), so that it is unlikely that the file still hasn’t been processed.

To obtain the file location use the

snippet:DefineFileLocationForDatabusFiles

snippet:FileLocationForDatabusFiles
 
## Configuring AzureDataBus

The following extension methods are available for changing the behavior of `AzureDataBus` defaults:

snippet:AzureDataBusSetup

 * `ConnectionString()`: the connection string to the storage account for storing DataBus properties, defaults to `UseDevelopmentStorage=true`
 * `Container()`: container name, defaults to '`databus`'
 * `BasePath()`: the blobs base path under the container, defaults to empty string
 * `DefaultTTL`: time in seconds to keep blob in storage before it is removed, defaults to `Int64.MaxValue` seconds
 * `MaxRetries`: number of upload/download retries, defaults to 5 retries
 * `NumberOfIOThreads`: number of blocks that will be simultaneously uploaded, defaults to 5 threads
 * `BackOffInterval`:  the back-off time between retries, defaults to 30 seconds
 * `BlockSize`: the size of a single block for upload when the number of IO threads is more than 1, defaults to 4MB
