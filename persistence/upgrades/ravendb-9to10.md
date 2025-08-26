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

NServiceBus.RavenDB version 10 introduces support for [RavenDB.Client 6.2.9 or higher](https://www.nuget.org/packages/RavenDB.Client).

RavenDB clients are forward-compatible starting with version 4.2, meaning a newer client can connect to a server of the same or higher version. However, if your servers are running a version prior to 6, upgrading the client requires extra care.

In this case, review both the [RavenDB client breaking changes](https://docs.ravendb.net/6.0/migration/client-api/client-breaking-changes/) and the [RavenDB server breaking changes](https://docs.ravendb.net/6.0/migration/server/server-breaking-changes/) to ensure compatibility before upgrading.

If your servers are already on RavenDB 6 or higher, you can safely upgrade the client library after consulting the [RavenDB client breaking changes](https://docs.ravendb.net/6.0/migration/client-api/client-breaking-changes/) without requiring immediate server changes.
