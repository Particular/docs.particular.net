---
title: Azure Storage Persistence Upgrade Version 6 to 7
summary: Instructions on how to migrate from Azure Table Persistence version 6 to 7
reviewed: 2025-09-10
component: ASP
related:
- persistence/azure-table
redirects:
isUpgradeGuide: true
upgradeGuideCoreVersions:
- 10
---

## Saga compatibility mode

[Configuring saga compatibility](/persistence/azure-table/configuration.md?version=astp_6#saga-compatibility-configuration) is no longer supported for saga instances persisted by NServiceBus.Persistence.AzureStorage versions 1 and 2. Legacy endpoint instances that have this configuration must be upgraded to remove it.
