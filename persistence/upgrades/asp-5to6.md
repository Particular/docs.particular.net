---
title: Azure Storage Persistence Upgrade Version 5 to 6
summary: Instructions on how to migrate from Azure Table Persistence version 5 to 6
reviewed: 2023-11-01
component: ASP
related:
- persistence/azure-table
redirects:
isUpgradeGuide: true
upgradeGuideCoreVersions:
- 9
---

## Saga compatibility mode

The [option to enable compatibility with saga instances persisted by NServiceBus.Persistence.AzureStorage version 1 and 2](/azure-table/configuration?#saga-compatibility-configuration&version=astp_5) is no longer available.
