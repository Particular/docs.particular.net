---
title: Upgrade from Azure Table persistence to Version 3
summary: Instructions on how to migrate from Azure Table Persistence v2 to v 3
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

The [timeout manager is removed from core](/nservicebus/upgrades/7to8/#timeout-manager-removed) which makes timeout storage obsolete.

- Any timeout-related configuration API's can be safely removed.
- Database tables must be manually removed from storage.

NOTE: There is no automatic migration of timeout data. See [Timeout manager removed - Data migration](/nservicebus/upgrades/7to8/#timeout-manager-removed-data-migration).
