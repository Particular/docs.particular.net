---
title: Azure Blob Storage DataBus
reviewed: 2016-08-29
component: ABSDataBus
tags:
 - DataBus
 - Attachments
related:
 - samples/azure/blob-storage-databus
---

Azure Blob Storage DataBus will **remove** the [Azure storage blobs](https://docs.microsoft.com/en-us/azure/storage/storage-dotnet-how-to-use-blobs) used for physical attachments after the message is processed if the `TimeToBeReceived` value is specified. When this value isn't provided, the physical attachments will not be removed.


## Usage

snippet:AzureDataBus


## Cleanup Strategy

Specify a proper value for the `TimeToBeReceived` property. For more details on how to specify this, read this article on [discarding old messages](/nservicebus/messaging/discard-old-messages.md).


## Configuration

The following extension methods are available for changing the behavior of `AzureDataBus` defaults:

snippet:AzureDataBusSetup

 * `ConnectionString()`: The connection string to the storage account for storing DataBus properties, defaults to `UseDevelopmentStorage=true`.
 * `Container()`: Container name, defaults to `databus`.
 * `BasePath()`: The blobs base path in the container, defaults to empty string.
 * `DefaultTTL`: Time in seconds to keep blob in storage before it is removed, defaults to [Int64.MaxValue](https://msdn.microsoft.com/en-us/library/system.int64.maxvalue.aspx) seconds.
 * `MaxRetries`: Number of upload/download retries, defaults to 5 retries.
 * `NumberOfIOThreads`: Number of blocks that will be simultaneously uploaded, defaults to 5 threads.
 * `BackOffInterval`: The back-off time between retries, defaults to 30 seconds.
 * `BlockSize`: The size of a single block for upload when the number of IO threads is more than 1, defaults to 4MB.