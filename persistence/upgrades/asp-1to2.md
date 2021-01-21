---
title: Azure Storage Persistence Upgrade Version 1 to 2
summary: Instructions on how to migrate from Azure Storage Persistence version 1 to version 2
reviewed: 2020-08-17
component: ASP
related:
 - persistence/azure-table
 - persistence/upgrades/asp-saga-deduplication
redirects:
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---

## Connections string should be set from code only

AppSetting `NServiceBus/Persistence` is no longer retrieved from the configuration file. Setting the connection string for timeouts and subscription persisters should be done via code.
