---
title: Azure Storage Persistence Upgrade Version 6 to 7
summary: Instructions on how to migrate from Azure Table Persistence version 6 to 7
reviewed: 2023-12-08
component: ASP
related:
- persistence/azure-table
redirects:
isUpgradeGuide: true
upgradeGuideCoreVersions:
- 9
---

## Saga compatibility mode

The [option to enable compatibility with saga instances persisted by NServiceBus.Persistence.AzureStorage version 1 and 2](/persistence/azure-table/configuration.md?version=astp_5#saga-compatibility-configuration) is no longer available and all legacy endpoint instances must be upgraded to no longer require it to be used.
