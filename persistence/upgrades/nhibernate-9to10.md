---
title: NHibernate Persistence Upgrade Version 9 to 10
summary: Migration instructions on how to upgrade the NHibernate persistence from version 9 to 10.
reviewed: 2023-10-26
component: NHibernate
related:
- persistence/nhibernate
- nservicebus/upgrades/8to9
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 9
---

## Timeout storage

The [timeout manager is removed from core](/nservicebus/upgrades/7to8/#timeout-manager-removed) which makes timeout storage obsolete.

- Any configuration APIs can safely be removed.
- Database tables must be manually removed from storage.

NOTE: There is no automatic migration of timeout data. See [Timeout manager removed - Data migration](/nservicebus/upgrades/7to8/#timeout-manager-removed-data-migration).
