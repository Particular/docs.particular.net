---
title: Azure Blob Storage Data Bus
summary: The Azure Blob Storage implementation of databus
reviewed: 2020-03-04
component: ABSDataBus
related:
 - samples/azure/blob-storage-databus
 - samples/azure/blob-storage-databus-cleanup-function
---

Azure Blob Storage Data Bus will **remove** the [Azure storage blobs](https://docs.microsoft.com/en-us/azure/storage/storage-dotnet-how-to-use-blobs) used for physical attachments after the message is processed if the `TimeToBeReceived` value is specified. When this value isn't provided, the physical attachments will not be removed.


## Usage

snippet: AzureDataBus

## Cleanup strategies

Discarding old Azure Data Bus attachments can be done in one of the following ways:

partial: cleanup-options

### Using the built-in clean-up method

Specify a value for the `TimeToBeReceived` property. For more details on how to specify this, see [Discarding Old Messages](/nservicebus/messaging/discard-old-messages.md).

WARN: The built-in method uses continuous blob scanning which can add to the cost of the storage operations. It is **not** recommended for multiple endpoints that are scaled out. If this method is not used, be sure to disable the built-in cleanup by setting the `CleanupInterval` to `0`. In versions 3 and above built-in cleanup is disabled by default.

### Using an Azure Durable Function

Review the [sample](/samples/azure/blob-storage-databus-cleanup-function/) to see how to use a durable function to clean up attachments.

### Using the Blob Lifecycle Management policy

Attachment blobs can be cleaned up using the [Blob Storage Lifecycle feature](https://docs.microsoft.com/en-us/azure/storage/blobs/storage-lifecycle-management-concepts). This method allows configuring a single policy for all data bus-related blobs. Those blobs can be either deleted or archived. The policy does not require custom code and is deployed directly to the storage account. This feature can only be used on GPv2 and Blob storage accounts, not on GPv1 accounts. 

## Configuration

partial: config

## Behavior

The following extension methods are available for changing the behavior of `AzureDataBus` defaults:

snippet: AzureDataBusSetup

partial: settings

### Disabling built-in blob cleanup

Setting the `CleanupInterval` to `0` will disable blob cleanup.

snippet: AzureDataBusDisableCleanup