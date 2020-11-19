---
title: SQL Persistence Upgrade Version 6 to 7
summary: Migration instructions on how to upgrade to SQL Persistence version 7
reviewed: 2020-11-06
component: SqlPersistence
related:
- persistence/sql
- nservicebus/upgrades/7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
---

## Timeout storage

The [timeout manager is removed from core](/nservicebus/upgrades/7to8/#timeout-manager-removed) which makes timeout storage obsolete.

- Any configuration APIs can safely be removed.
- Database tables must be manually removed from storage.

NOTE: There is no automatic migration of timeout data. See [Timeout manager removed - Data migration](/nservicebus/upgrades/7to8/#timeout-manager-removed-data-migration).
