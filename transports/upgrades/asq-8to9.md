---
title: Azure Storage Queues Transport Upgrade Version 8 to 9
summary: Migration instructions on how to upgrade Azure Storage Queues Transport from Version 8 to 9.
reviewed: 2029-11-09
component: ASQ
related:
- transports/azure-storage-queues
- nservicebus/upgrades/7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
---

## Timeout manager

The [timeout manager is removed from core](/nservicebus/upgrades/7to8/#timeout-manager-removed) which makes timeout manager backwards compatibility mode obsolete. If backwards compatibility mode was enabled these API's must be removed.