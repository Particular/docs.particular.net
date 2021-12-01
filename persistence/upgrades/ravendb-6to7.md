---
title: RavenDB Persistence Upgrade from 6 to 7
summary: Migration instructions on how to upgrade NServiceBus.RavenDB 6 to 7
component: Raven
related:
- persistence/ravendb
- nservicebus/upgrades/6to7
reviewed: 2020-02-11
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

## Support for RavenDB.Client version 5.2 and higher

Starting with NServiceBus.RavenDB version 7.0.0 [RavenDB.Client](https://www.nuget.org/packages/RavenDB.Client/) version 5.2.1 or higher is required. For more information about the client and server changes visit the [official RavenDB migration guide](https://ravendb.net/docs/article-page/5.0/csharp/migration).

## Cluster-wide transactions

[Cluster-wide transactions](https://ravendb.net/docs/article-page/5.2/start/server/clustering/cluster-transactions) are supported, enabling the use of RavenDB clusters and database groups replicated across multiple nodes. In case of cluster configurations the cluster-wide transaction mode needs to be enabled:

snippet: ravendb-persistence-cluster-wide-transactions

## Gateway deduplication storage

Starting with NServiceBus.RavenDB version 6.2.0, the built-in gateway API is obsolete and using it will produce the following message:

> RavenDB gateway persistence has been moved to the NServiceBus.Gateway.RavenDB dedicated package. Will be treated as an error from version 7.0.0. Will be removed in version 8.0.0.

To migrate to the new gateway API:

- Add a reference to the [NServiceBus.Gateway.RavenDB](https://www.nuget.org/packages/NServiceBus.Gateway.RavenDB) NuGet package and configure the gateway feature following the instructions available in the [RavenDB Gateway Storage documentation](/nservicebus/gateway/ravendb/).
- Remove any configuration that uses the legacy gateway API.