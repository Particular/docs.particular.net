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

partial: cleanup-setup

## Configuration

partial: config

## Behavior

The following extension methods are available for changing the behavior of `AzureDataBus` defaults:

snippet: AzureDataBusSetup

partial: settings

partial: disable-cleanup