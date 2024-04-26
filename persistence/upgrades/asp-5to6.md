---
title: Azure Storage Persistence Upgrade Version 5 to 6
summary: Instructions on how to migrate from Azure Table Persistence version 5 to 6
reviewed: 2024-04-26
component: ASP
related:
- persistence/azure-table
redirects:
isUpgradeGuide: true
upgradeGuideCoreVersions:
- 9
---

## Saga compatibility mode

[Configuring saga compatibility](/persistence/azure-table/configuration.md?version=astp_5#saga-compatibility-configuration) is no longer supported for saga instances persisted by NServiceBus.Persistence.AzureStorage versions 1 and 2. Legacy endpoint instances that have this configuration must be upgraded to remove it.
