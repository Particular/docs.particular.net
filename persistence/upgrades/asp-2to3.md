---
title: Upgrade from Azure Storage persistence to Version 3
summary: Instructions on how to migrate from Azure Storage Persistence to version 2
reviewed: 2020-11-06
component: ASP
related:
 - persistence/azure-storage
redirects:
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
---

## Timeout storage

The [timeout manager is removed from core](/nservicebus/upgrades/7to8/#timeout-manager-removed) which makes timeout storage obsolete.

- Any configuration API's can safely be removed.
- Database tables must be manually removed from storage.

NOTE: There is no automatic migration of timeout data. See [Timeout manager removed - Data migration](/nservicebus/upgrades/7to8/#timeout-manager-removed-data-migration).