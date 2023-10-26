---
title: NHibernate Persistence Upgrade Version 8 to 9
summary: Migration instructions on how to upgrade the NHibernate persistence from version 8 to 9.
reviewed: 2022-10-19
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

Timeout storage is being deprecated, [see the Verion 9 to 10 upgrade guide for more details](/persistence/upgrades/nhibernate-9to10.md#timeout-storage).
