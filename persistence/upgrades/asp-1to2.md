---
title: Upgrade from Azure Storage persistence Version 1 to Verersion 2
summary: Instructions on how to migrate from NServiceBus.Azure Storage Persistence Version 6 to NServiceBus.Persistence.AzureStorage Version 1.
reviewed: 2017-08-31
component: ASP
related:
 - persistence/azure-storage
 - persistence/upgrades/asp-saga-deduplication
redirects:
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---


## Connections string should be set from code only

AppSetting `NServiceBus/Persistence` is no longer retrieved from the configuration file. Setting connection string for Timeouts and Subscription persisters should be done via code.

