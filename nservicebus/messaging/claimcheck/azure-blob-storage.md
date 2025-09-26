---
title: Azure Blob Storage Data Bus
summary: The Azure Blob Storage implementation of databus
reviewed: 2024-02-16
component: ABSDataBus
redirects:
 - nservicebus/messaging/databus/azure-blog-storage
related:
 - samples/databus/blob-storage-databus
 - samples/databus/blob-storage-databus-cleanup-function
---

## Usage

snippet: AzureDataBus

## Cleanup strategies

Discarding old Azure Data Bus attachments can be done in one of the following ways:

partial: cleanup-options

include: azure-blob-storage-management-policy

## Configuration

partial: config

## Behavior

The following extension methods are available for changing the behavior of `AzureDataBus` defaults:

snippet: AzureDataBusSetup

partial: settings