---
title: Moving to the DataBus AzureBlobStorage package
reviewed: 2016-11-05
tags:
 - upgrade
 - migration
related:
 - nservicebus/upgrades/azure-deprecation
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---

Instructions on how to move from the [NServiceBus.Azure NuGet package](https://www.nuget.org/packages/NServiceBus.Azure/) to the [NServiceBus.DataBus.AzureBlobStorage NuGet package](https://www.nuget.org/packages/NServiceBus.DataBus.AzureBlobStorage/).


### AzureDataBusPersistence feature no longer used

Instead of `.EnableFeature<AzureDataBusPersistence>()` use `.UseDataBus<AzureDataBus>()`.


### Custom configuration section no longer provided

Configuration options are now only available via the code API. See [Azure Blob Storage DataBus](/nservicebus/messaging/databus/azure-blob-storage.md) and [Azure DataBus sample](/samples/azure/blob-storage-databus) for more details.


### AzureDataBusDefaults no longer provided

Refer to the [Azure Blob Storage DataBus](/nservicebus/messaging/databus/azure-blob-storage.md) for details on defaults used.
