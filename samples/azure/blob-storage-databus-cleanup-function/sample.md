---
title: Azure Blob Storage Data Bus Cleanup with Azure Functions
summary: Using an Azure Function instead of the built in blob cleanup capabilities.
component: ABSDataBus
reviewed: 2020-04-07
related:
- nservicebus/messaging/databus
---

This sample shows how to use [Azure Functions](https://azure.microsoft.com/en-us/services/functions/) to automatically trigger blob cleanup. 

downloadbutton

## Prerequisites

 1. Make sure [Azure Functions Tools for Visual Studio](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs#prerequisites) are setup correctly.
 1. Start [Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/storage-use-emulator). Ensure the [latest version](https://go.microsoft.com/fwlink/?linkid=717179&clcid=0x409) is installed.
 1. Run the solution. Two console applications start.
 1. Find the `SenderAndReceiver` application by looking for the one with `SenderAndReceiver` in its path and press <kdb>enter</kbd> to send a large message. NServiceBus sends it as an attachment via Azure storage. The `DataBusBlobCreated` trigger function runs in the Function window, followed by the `DataBusCleanupOrchestrator` orchestrator function, invoking the `DeleteBlob` activity function, deleting the blob when the time-to-live for the message is reached.

## Code walk-through

This sample contains two projects:

 * DataBusBlobCleanupFunctions - An Azure Function project that contains the three Azure Functions that perform the cleanup. 
 * SenderAndReceiver - A console application responsible for sending and receiving the large message.

### DatabusBlobCleanupFunctions

#### DataBusBlobCreated

The following Azure Function is included in this project that is triggered when a blob is created or updated in the data bus path in the storage account.

snippet: DataBusBlobCreatedFunction

The execution uses a [singleton orchestration](https://docs.microsoft.com/en-us/azure/azure-functions/durable-functions-singletons) pattern using the blob name when starting the `DataBusCleanupOrchestrator` function. This prevents multiple timeouts from being started.

The `GetValidUntil` method uses logic that reproduces the cleanup functionality of the `NServiceBus.DataBus.AzureBlobStorage` package. 

snippet: GetValidUntil

The method evaluates the metadata of the blob looking for previously provided timeout values. If none are found the default time to live is calculated for the blob and returned.

The timeout value is passed in when the `DataBusCleanupOrchestrator` orchestration function is executed.

#### DataBusCleanupOrchestrator

snippet: DataBusCleanupOrchestratorFunction

The function uses a [durable function timer](https://docs.microsoft.com/en-us/azure/azure-functions/durable-functions-timers) to delay execute deletion of the blob from azure storage.

partial: delete

#### Configuring time to live for large binary objects

The default time to live for all large binary objects is configured by setting the `DefaultTimeToLiveInSeconds` environment variable. This can be set during debugging by adding the appropriate `Values` setting in the `local.settings.json` file: 

```json
{
  "IsEncrypted": false,
  "Values": {
     "DefaultTimeToLiveInSeconds":  "180"
  }
}
```

In production this is set using an [applications settings](https://docs.microsoft.com/en-us/azure/azure-functions/functions-how-to-use-azure-function-app-settings#settings) value named `DefaultTimeToLiveInSeconds` in the [Azure portal](https://portal.azure.com).

A message with a set [time to be received](/nservicebus/messaging/discard-old-messages.md) will override the default time to live for the large binary object and instead use this value when determining the time to clean up the blob.

#### Configuring the data bus location

The `DataBusBlobCleanupFunctions` project needs to access the large binary objects. This is done by specifying an Azure storage connection string in the `DataBusStorageAccount` environment variable. This can be set during debugging by adding the appropriate `Values` setting in the `local.settings.json` file: 

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

In environments where `NServiceBus.DataBus.AzureBlobStorage` is already in use the timeout function will need to be triggered for the existing attachments.

A manually-triggered function called `DataBusOrchestrateExistingBlobs` is included to trigger orchestration for every existing blob in the container. It's an HTTP triggered function that can be invoked using a browser.

snippet: DataBusOrchestrateExistingBlobsFunction

The function is very similar to the [`DataBusBlobCreated`](#code-walk-through-databusblobcleanupfunctions-databusblobcreated) function, but instead of working on a single blob at a time it will iterate over every existing blob in the container.

This function does not require downtime as the implemented [singleton orchestration](https://docs.microsoft.com/en-us/azure/azure-functions/durable-functions-singletons) pattern will prevent existing timeouts from being duplicated.

### SenderAndReceiver project

The project sends the `MessageWithLargePayload` message to itself, utilizing the NServiceBus attachment mechanism.

partial: cleanup