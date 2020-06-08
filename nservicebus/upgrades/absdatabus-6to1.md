---
title: Moving to the DataBus AzureBlobStorage Package
summary: Instructions on how to move from the NServiceBus.Azure package to NServiceBus.DataBus.AzureBlobStorage
reviewed: 2020-06-08
component: ABSDataBus
related:
 - nservicebus/upgrades/azure-deprecation
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---

This page provides instructions on how to move from the [NServiceBus.Azure NuGet package](https://www.nuget.org/packages/NServiceBus.Azure/) to the [NServiceBus.DataBus.AzureBlobStorage NuGet package](https://www.nuget.org/packages/NServiceBus.DataBus.AzureBlobStorage/).


### AzureDataBusPersistence feature no longer used

Instead of `.EnableFeature<AzureDataBusPersistence>()` use `.UseDataBus<AzureDataBus>()`.


### Custom configuration section no longer provided

Configuration options are now available only via the code API. See [Azure Blob Storage Data Bus](/nservicebus/messaging/databus/azure-blob-storage.md) and the [Azure Data Bus sample](/samples/azure/blob-storage-databus) for more details.


### AzureDataBusDefaults no longer provided

Refer to the [Azure Blob Storage Data Bus](/nservicebus/messaging/databus/azure-blob-storage.md) for details on defaults used.
