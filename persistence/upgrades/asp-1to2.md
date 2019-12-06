---
title: Upgrade from Azure Storage persistence Version 1 to Verersion 2
summary: Instructions on how to migrate from Azure Storage Persistence version 1 to version 2
reviewed: 2018-12-05
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

AppSetting `NServiceBus/Persistence` is no longer retrieved from the configuration file. Setting the connection string for timeouts and subscription persisters should be done via code.

