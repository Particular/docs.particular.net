---
title: Upgrade Azure Storage Persistence to Version 3
summary: Instructions on how to migrate from Azure Storage Persistence version 2 to version 3
reviewed: 2020-11-06
component: ASP
related:
 - persistence/azure-table
redirects:
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
---

## Timeout storage

The [timeout manager has been removed from core](/nservicebus/upgrades/7to8/#timeout-manager-removed) which makes timeout storage obsolete.

- Any configuration APIs can safely be removed.
- Database tables must be manually removed from storage.

NOTE: There is no automatic migration of timeout data. See [Timeout manager removed - Data migration](/nservicebus/upgrades/7to8/#timeout-manager-removed-data-migration).