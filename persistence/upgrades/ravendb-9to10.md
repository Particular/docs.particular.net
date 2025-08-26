---
title: RavenDB Persistence Upgrade from 9 to 10
summary: Migration instructions on how to upgrade NServiceBus.RavenDB 9 to 10
component: Raven
related:
- persistence/ravendb
reviewed: 2025-08-25
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 9
---

## RavenDB Client Upgrade

NServiceBus.RavenDB version 10.0.0 introduces support for RavenDB.Client 6.*.
Starting with RavenDB.Client version 4.2, RavenDB clients are forward-compatible with any server of the same version or higher. This means you can safely upgrade your client library without requiring immediate server changes.
For a comprehensive list of breaking changes and migration steps, refer to the [official RavenDB client migration guide.](https://docs.ravendb.net/6.0/migration/client-api/client-breaking-changes/)