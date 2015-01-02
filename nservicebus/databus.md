---
title: DataBus feature
summary: DataBus feature
tags:
- DataBus
- Attachments
---

## What is DataBus feature for?

Messages are intended to be small. Some scenarios require to send large binary data along with a message. For this purpose, NServiceBus has a DataBus feature to allow to overcome limitations of message size.

## How it works?

`DataBus` is taking an approach of storing large payload and storing it in at a location both sending and receiving parties can access. Message is sent with a reference to the location, and upon processing payload is brought, allowing receiving part access message along with payload. In case location is not available upon sending, message will fail send operation. Receive operation will fail as well when payload location is not available and will cause standard NServiceBus behaviour, causing retries and eventually going into error queue.

## How to enable DataBus

NServiceBus supports 2 DataBus implementations:

* `FileShareDataBus`
* `AzureDataBus`

To enable DataBus, NServiceBus needs to be configured. For file share based DataBus:
<!-- import FileShareDataBus -->

For Azure (storage blobs) based DataBus:
<!-- import AzureDataBus -->

Note: The `AzureDataBus` implementation is part of the Azure transport package.

## Specifying message properties for DataBus

There are two ways to specify message properties to be sent using DataBus
1. Using `DataBusProperty<T>` type
2. Message conventions

### Using DataBusProperty<T>

Properties defined using `DataBusProperty<T>` type provided by NServiceBus will not be treated as part of a message, but persisted externally based on type of `DataBus` used and linked to the original message using a unique key. 

<!-- import MessageWithLargePayload -->

### Using message conventions

NServiceBus supports [message conventions feature](/nservicebus/unobtrusive-sample.md). This feature allows to define convention for data properties to be sent using `DataBus` without referencing NServiceBus specific types like `DataBusProperty<T>`.

<!-- import DefineMessageWithLargePayloadUsingConvention -->

<!-- import MessageWithLargePayloadUsingConvention -->

##DataBus attachements cleanup

NServiceBus `DataBus` implementations currently behave differently in regards to cleanup of physical attachments used to transfer data properties. `FileShareDataBus` **does not** remove physical attachments once message is gone. `AzureDataBus` **does** remove Azure storage blobs used for physical attachments once message is gone.

## Configuring AzureDataBus

The following extension methods are available for changing the behaviour of `AzureDataBus` defaults:

<!-- import AzureDataBusConfiguration -->

- `ConnectionString()`: set the connection string to the storage account for storing DataBus properties, defaults to `UseDevelopmentStorage=true`
- `Container()`: set the container name, defaults to '`databus`'
- `BasePath()`: set blobs base path under container, defaults to empty string
- `DefaultTTL`: set time in seconds to keep blob in storage before it's removed, defaults to `Int64.MaxValue` seconds
- `MaxRetries`: set of upload/download retries, defaults to 5 retries
- `NumberOfIOThreads`: set number of blocks that will be simultaneously uploaded , defaults to 5 threads
- `BackOffInterval`:  set back-off time time between retries, defaults to 30 seconds
<!-- NOT USED. See https://github.com/Particular/NServiceBus.Azure/issues/236   - `BlockSize`: set size of a single block for upload when number of IO threads is more than 1 , defaults to 4MB -->
