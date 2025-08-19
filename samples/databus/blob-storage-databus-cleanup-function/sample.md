---
title: Azure Blob Storage Data Bus Cleanup with Azure Functions
summary: Using an Azure Function instead of the built in blob cleanup capabilities.
component: ABSDataBus
reviewed: 2025-07-25
related:
- nservicebus/messaging/claimcheck
redirects:
- samples/azure/blob-storage-databus-cleanup-function
---

This sample shows how to use [Azure Functions](https://azure.microsoft.com/en-us/services/functions/) to automatically trigger blob cleanup.

downloadbutton

## Prerequisites

1. [Azure Functions Tools for Visual Studio](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs#prerequisites)
1. [Azurite Emulator](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio)

## Running the sample

1. Start [Azurite Emulator](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio).
1. Run the solution—two console applications will start.
1. Switch to the console window with `SenderAndReceiver` in its path, and press <kbd>enter</kbd> to send a large message.

## Code walk-through

This sample contains two projects:

- SenderAndReceiver—a console application which sends and receives a large message.
- DataBusBlobCleanupFunctions—an Azure Functions project with three Azure Functions that perform cleanup.

### SenderAndReceiver

This project sends a `MessageWithLargePayload` to itself. The message is sent using an attachment stored in Azure Storage.

### DatabusBlobCleanupFunctions

#### DataBusBlobCreated

This Azure Function is triggered when a blob is created or updated in the data bus path in the storage account.

snippet: DataBusBlobCreatedFunction

To prevent multiple timeouts from starting, the function uses the [singleton orchestration](https://docs.microsoft.com/en-us/azure/azure-functions/durable-functions-singletons) pattern, using the blob name, when starting the `DataBusCleanupOrchestrator` function.

The `GetValidUntil` method imitates the behavior of the `NServiceBus.DataBus.AzureBlobStorage` package.

snippet: GetValidUntil

The method looks for a previously set timeout value in the blob metadata. If none is found, the default time to live (`DateTime.MaxValue`) is returned.

The timeout value is passed to the `DataBusCleanupOrchestrator` function.

#### DataBusCleanupOrchestrator

snippet: DataBusCleanupOrchestratorFunction

The function uses a [durable function timer](https://docs.microsoft.com/en-us/azure/azure-functions/durable-functions-timers) to delete the blob from Azure Storage after the timeout period has elapsed.

#### DeleteBlob

snippet: DeleteBlobFunction

The function is executing the actual work to delete a blob.

#### Configuring time to live for large binary objects

If a message has a specific [time to be received](/nservicebus/messaging/discard-old-messages.md), that value will be used to determine when to clean up the blob.

#### Configuring the data bus location

The `DataBusBlobCleanupFunctions` project requires access to the large binary objects. This is provided by an Azure Storage connection string in the `DataBusStorageAccount` environment variable. This can be set during debugging by adding the appropriate `Values` setting in the `local.settings.json` file:

```json
{
  "IsEncrypted": false,
  "Values": {
    "DataBusStorageAccount": "UseDevelopmentStorage=true"
  }
}
```

In production this is set using an [applications settings](https://docs.microsoft.com/en-us/azure/azure-functions/functions-how-to-use-azure-function-app-settings#settings) value named `DataBusStorageAccount` in the [Azure portal](https://portal.azure.com).

#### Migrating existing projects

In environments where `NServiceBus.DataBus.AzureBlobStorage` is already in use, the timeout function must be triggered for the existing attachments.

`DataBusOrchestrateExistingBlobs` is used to trigger orchestration for every existing blob in the container. It's an HTTP triggered function that can be invoked manually using a browser.

snippet: DataBusOrchestrateExistingBlobsFunction

The function is very similar to the [`DataBusBlobCreated`](#code-walk-through-databusblobcleanupfunctions-databusblobcreated) function, but instead of working on a single blob, it iterates over every blob in the container.

This function does not require downtime as the implemented [singleton orchestration](https://docs.microsoft.com/en-us/azure/azure-functions/durable-functions-singletons) pattern prevents existing timeouts from being duplicated.
