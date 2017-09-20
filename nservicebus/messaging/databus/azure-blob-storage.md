---
title: Azure Blob Storage DataBus
reviewed: 2016-08-29
component: ABSDataBus
tags:
 - DataBus
related:
 - samples/azure/blob-storage-databus
---

Azure Blob Storage DataBus will **remove** the [Azure storage blobs](https://docs.microsoft.com/en-us/azure/storage/storage-dotnet-how-to-use-blobs) used for physical attachments after the message is processed if the `TimeToBeReceived` value is specified. When this value isn't provided, the physical attachments will not be removed.


## Usage

snippet: AzureDataBus


## Cleanup Strategy

Specify a proper value for the `TimeToBeReceived` property. For more details on how to specify this, read this article on [discarding old messages](/nservicebus/messaging/discard-old-messages.md).


## Configuration

The following extension methods are available for changing the behavior of `AzureDataBus` defaults:

snippet: AzureDataBusSetup

partial: settings
