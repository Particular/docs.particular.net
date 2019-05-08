---
title: Azure Blob Storage DataBus
summary: The Azure Blob Storage implementation of databus
reviewed: 2018-04-10
component: ABSDataBus
tags:
 - DataBus
related:
 - samples/azure/blob-storage-databus
 - samples/azure/blob-storage-databus-cleanup-function
---

Azure Blob Storage DataBus will **remove** the [Azure storage blobs](https://docs.microsoft.com/en-us/azure/storage/storage-dotnet-how-to-use-blobs) used for physical attachments after the message is processed if the `TimeToBeReceived` value is specified. When this value isn't provided, the physical attachments will not be removed.


## Usage

snippet: AzureDataBus


## Cleanup strategies

Discarding old Azure DataBus attachments can be done in one of the following ways:

1. Using the built-in method (enabled by default)
2. Using an Azure Durable Function
3. Using the Blob Lifecycle Management policy

### Using the built-in clean-up method

Specify a value for the `TimeToBeReceived` property. For more details on how to specify this, see [Discarding Old Messages](/nservicebus/messaging/discard-old-messages.md).

WARN: the built-in method uses continuous blob scanning which can add to the cost of the storage operations. It is **not** recommended for multiple endpoints that are scaled out. If this method is not used, be sure to disable the built-in cleanup by setting the `CleanupInterval` to `0`.

### Using an Azure Durable Function

Review our [sample](/sampls/azure/blob-storage-database-cleanup-function/) to see how to use a durable function to clean up attachments. Be sure to [disable blob cleanup](#disabling-blob-cleanup) first.

### Using the Blob Lifecycle Management policy

Attachment blobs can be cleaned up using the [Blob Storage Lifecycle feature](https://docs.microsoft.com/en-us/azure/storage/blobs/storage-lifecycle-management-concepts). This method allows configuring a single policy for all DataBus-related blobs. Those blobs can be either deleted or archived. The policy does not require custom code and is deployed directly to the storage account. This feature can only be used on GPv2 and Blob storage accounts, not on GPv1 accounts. 


## Configuration

The following extension methods are available for changing the behavior of `AzureDataBus` defaults:

snippet: AzureDataBusSetup

partial: settings

### Disabling blob cleanup

Setting the `CleanupInterval` to `0` will disable blob cleanup.

snippet: AzureDataBusDisableCleanup
