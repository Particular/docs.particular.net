---
title: RavenDB Persistence Upgrade from 7 to 8
summary: Migration instructions on how to upgrade NServiceBus.RavenDB 7 to 8
component: Raven
related:
- persistence/ravendb
- nservicebus/upgrades/7to8
reviewed: 2021-12-03
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

## Timeout storage

The [timeout manager is removed from core](/nservicebus/upgrades/7to8/#timeout-manager-removed) which makes timeout storage obsolete.

- Any configuration APIs can safely be removed.
- Database tables must be manually removed from storage.

NOTE: There is no automatic migration of timeout data. See [Timeout manager removed - Data migration](/nservicebus/upgrades/7to8/#timeout-manager-removed-data-migration).
