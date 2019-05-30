---
title: RavenDB Persistence Upgrade from 5 to 6
summary: Instructions on how to upgrade NServiceBus.RavenDB 5 to 6
component: Raven
related:
 - nservicebus/upgrades/6to7
reviewed: 2019-05-30
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

TODO: Remove this diff link: https://github.com/Particular/NServiceBus.RavenDB/compare/tmp-start-of-6...develop

include: dtc-warning

This update supports upgrading to RavenDB 4.2. The upgrade to RavenDB 4 is a **major** upgrade. It's not backward compatible, and the [API has a lot of breaking changes](https://ravendb.net/docs/article-page/4.2/csharp/migration/client-api/introduction). Additionally, the data storage format has changed, so databases need to have their [data migrated to a new RavenDB 4 database](https://ravendb.net/docs/article-page/4.2/csharp/migration/server/data-migration).

## Required RavenDB client

RavenDB Persistence requires [RavenDB.Client 4.2.0](https://www.nuget.org/packages/RavenDB.Client/4.2.0) or later.


## Supported cluster configurations

RavenDB 4 [does not support Windows Failover Clusters](https://groups.google.com/forum/#!msg/ravendb/_TxAFNlCXik/n1RS_m-SAwAJ) and instead has built-in support for [RavenDB Clustering](https://ravendb.net/docs/article-page/4.2/csharp/server/clustering/overview).

When using RavenDB clustering, only clusters with one full Member node (the Leader) are supported. In multi-member clusters, NServiceBus can't store its data without causing replication conflicts. On startup, NServiceBus will retrieve the cluster topology and throw an exception if the cluster contains more than one Member node:

> RavenDB Persistence does not support RavenDB clusters with more than one Leader/Member node. Only clusters with a single Leader and (optionally) Watcher nodes are supported.

The cluster can support multiple Watcher nodes.

See RavenDB's [Cluster Nodes Types](https://ravendb.net/docs/article-page/4.2/csharp/studio/server/cluster/cluster-view#cluster-nodes-types) documentation for more information.


## Storage format for sagas

stub

https://github.com/Particular/NServiceBus.RavenDB/compare/tmp-start-of-6...develop#diff-d062550628f1318f3b80ca634c8a2fd6R1


## Custom saga finders no longer supported

stub

https://github.com/Particular/NServiceBus.RavenDB/compare/tmp-start-of-6...develop#diff-025f265cbac62cad20d1568b1ad70f56R18


## No connecting via `ConnectionParameters` or connection strings

The `ConnectionParameters` class contained details for creating a RavenDB `DocumentStore`, but most of the properties are no longer applicable due to the breaking changes in RavenDB 4.0. As a result, the `ConnectionParameters` class is deprecated and a fully-configured `DocumentStore` must be provided to NServiceBus instead. See [Connection Options](/persistence/ravendb/connection.md?version=raven_6) for supported connection options in NServiceBus.RavenDB 6.

TODO: Connection strings too
TODO: And default localhost:8080 DocStore


## Subscription versioning options removed

stub

* Deprecated  `persistence.DisableSubscriptionVersioning()` and `persistence.UseLegacyVersionedSubscriptions()`, throws `NotImplementedException`


## Legacy document conventions cannot be used

Stub

* Using DocStore conventions
* Deprecated `persistence.DoNotUseLegacyConventionsWhichIsOnlySafeForNewEndpoints()`, throws `NotImplementedException`
* Saga id conventions
* Timeout id conventions


## Legacy Outbox document id format no longer used

TODO: Look up version when the change to add endpoint was made


## No longer converting V4 core timeouts

Stub

TODO: Look up TimeoutData converter stuff in code


## Updated .NET Framework versions

Because the [RavenDB.Client 4.2.0 NuGet package](https://www.nuget.org/packages/RavenDB.Client/4.2.0) is shipped for only the `netstandard20` and `netcoreapp2.1` build targets, RavenDB persistence can now only support frameworks implementing .NET Standard 2.0:

* .NET Core 2.0 and above
* .NET Framework 4.7.2 and above