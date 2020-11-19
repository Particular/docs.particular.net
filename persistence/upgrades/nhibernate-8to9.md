---
title: NHibernate Persistence Upgrade Version 8 to 9
summary: Migration instructions on how to upgrade the NHibernate persistence from version 8 to 9.
reviewed: 2020-02-11
component: NHibernate
related:
- persistence/nhibernate
- nservicebus/upgrades/7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
---

## Gateway deduplication storage

Starting with NServiceBus.NHibernate version 8.3.0, the built-in gateway API is obsolete and using it will produce the following message:

> NHibernate gateway persistence is deprecated. Use the new NServiceBus.Gateway.Sql dedicated package. Will be treated as an error from version 9.0.0. Will be removed in version 10.0.0.

To migrate to the new gateway API:

- Add a reference to the [NServiceBus.Gateway.Sql](https://www.nuget.org/packages/NServiceBus.Gateway.Sql) NuGet package and configure the gateway feature following the instructions available in the [SQL Gateway Storage documentation](/nservicebus/gateway/sql/).
- Remove any configuration that uses the legacy gateway API.

## Timeout storage

The [timeout manager is removed from core](/nservicebus/upgrades/7to8/#timeout-manager-removed) which makes timeout storage obsolete.

- Any configuration APIs can safely be removed.
- Database tables must be manually removed from storage.

NOTE: There is no automatic migration of timeout data. See [Timeout manager removed - Data migration](/nservicebus/upgrades/7to8/#timeout-manager-removed-data-migration).
