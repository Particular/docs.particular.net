---
title: Moving to NServiceBus.DataBus.AzureBlobStorage package
summary: Instructions on how to move from the NServiceBus.Azure package to the NServiceBus.DataBus.AzureBlobStorage package.
tags:
 - upgrade
 - migration
related:
- nservicebus/upgrades/azure-deprecation
---

Instructions on how to move from the NServiceBus.Azure package to the NServiceBus.DataBus.AzureBlobStorage package.

### AzureDataBusPersistence feature no longer used

Instead of `.EnableFeature<AzureDataBusPersistence>()` use `.UseDataBus<AzureDataBus>()` instead.

### Custom configuration section no longer provided

Configuration options are now only available via the code api. See [DataBus](/nservicebus/messaging/databus.md) documentation and the [Azure DataBus sample](/samples/azure/blob-storage-databus) for more details.

### AzureDataBusDefaults no longer provided

Refer to the [DataBus](/nservicebus/messaging/databus.md) documentation for details on defaults used.