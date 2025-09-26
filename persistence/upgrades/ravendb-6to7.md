---
title: RavenDB Persistence Upgrade from 6 to 7
summary: Migration instructions on how to upgrade NServiceBus.RavenDB 6 to 7
component: Raven
related:
- persistence/ravendb
- nservicebus/upgrades/6to7
reviewed: 2021-12-03
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

## Support for RavenDB.Client version 5.2 and higher

Starting with NServiceBus.RavenDB version 7.0.0 [RavenDB.Client](https://www.nuget.org/packages/RavenDB.Client/) version 5.2.1 or higher is required. For more information about the client and server changes refer to the [official RavenDB migration guide](https://ravendb.net/docs/article-page/5.0/csharp/migration).

## Pessimistic concurrency

Up to and including version 6.5, the persistence uses [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) by default when updating or deleting saga data. In most cases pessimistic concurrency control will improve performance, but in some edge cases optimistic concurrency control can be much faster. It is recommended to performance test if upgrading might cause issues. To enable optimistic concurrency use:

snippet: ravendb-persistence-optimistic-concurrency-6to7

## Outbox cleaner disabled by default

Starting with NServiceBus.RavenDB version 6.3, it is recommended to rely on [document expiration](https://ravendb.net/docs/article-page/latest/csharp/server/extensions/expiration) for outbox cleanup. The outbox cleaner is disabled by default from version 7. If the cleaner was previously disabled as shown in the following snippet, the code can be safely removed.

snippet: OutboxRavendBDisableCleanup6to7

For more information, refer to the [outbox cleanup guidance](/persistence/ravendb/outbox.md#deduplication-record-lifespan).

## Cluster-wide transactions

[Cluster-wide transactions](https://ravendb.net/docs/article-page/5.2/start/server/clustering/cluster-transactions) are supported, enabling the use of RavenDB clusters and database groups replicated across multiple nodes. To turn on cluster-wide transaction support use the following configuration:

snippet: ravendb-persistence-cluster-wide-transactions-6to7

## Gateway deduplication storage

Starting with NServiceBus.RavenDB version 6.2.0, the built-in gateway API is obsolete and using it will produce the following message:

> RavenDB gateway persistence has been moved to the NServiceBus.Gateway.RavenDB dedicated package. Will be treated as an error from version 7.0.0. Will be removed in version 8.0.0.

To migrate to the new gateway API:

- Add a reference to the [NServiceBus.Gateway.RavenDB](https://www.nuget.org/packages/NServiceBus.Gateway.RavenDB) NuGet package and configure the gateway feature following the instructions available in the [RavenDB Gateway Storage documentation](/nservicebus/gateway/ravendb/).
- Remove any configuration that uses the legacy gateway API.
