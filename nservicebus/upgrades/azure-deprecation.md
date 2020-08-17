---
title: NServiceBus.Azure package deprecated
summary: Instructions on how to move from the NServiceBus.Azure package to the new individual packages.
reviewed: 2020-08-17
related:
 - nservicebus/upgrades/5to6
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---

`NServiceBus.Azure` package is no longer provided. All functionality has been moved to individual packages, as listed below.

* Persistence - `NServiceBus.Persistence.AzureStorage` see the [Azure Storage Persistence upgrade guide](/persistence/upgrades/asp-6to1.md) for more details.
* Data Bus - `NServiceBus.DataBus.AzureBlobStorage` see the [Azure Blob Storage Data Bus upgrade guide](/nservicebus/upgrades/absdatabus-6to1.md) for more details.
