---
title: NServiceBus.Azure package deprecated
summary: Instructions on how to move from the NServiceBus.Azure package to the new individual packages.
tags:
 - upgrade
 - migration
related:
- nservicebus/upgrades/5to6
- nservicebus/upgrades/absdatabus-6to1
---

## NServiceBus.Azure package deprecated

`NServiceBus.Azure` package is no longer provided. All functionality has been moved to individual packages as listed below.

* Persistence - `NServiceBus.Persistence.AzureStorage` see the [Azure Storage Persistence upgrade guide](/nservicebus/upgrades/asp-6to1.md) for more details.
* DataBus - `NServiceBus.DataBus.AzureBlobStorage` see the [Azure Blob Storage DataBus upgrade guide](/nservicebus/upgrades/absdatabus-6to1.md) for more details.
