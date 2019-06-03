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

This update supports upgrading to RavenDB 4.2 while using NServiceBus 7.x. The upgrade to RavenDB 4 is a **major** upgrade. It's not backward compatible, and the [API has a lot of breaking changes](https://ravendb.net/docs/article-page/4.2/csharp/migration/client-api/introduction). Additionally, the data storage format has changed, so databases need to have their [data migrated to a new RavenDB 4 database](https://ravendb.net/docs/article-page/4.2/csharp/migration/server/data-migration).

## Required RavenDB client

RavenDB Persistence requires [RavenDB.Client 4.2.0](https://www.nuget.org/packages/RavenDB.Client/4.2.0) or later.


## Supported cluster configurations

RavenDB 4 [does not support Windows Failover Clusters](https://groups.google.com/forum/#!msg/ravendb/_TxAFNlCXik/n1RS_m-SAwAJ) and instead has built-in support for [RavenDB Clustering](https://ravendb.net/docs/article-page/4.2/csharp/server/clustering/overview).

When using RavenDB clustering, only clusters with one full Member node (the Leader) are supported. In multi-member clusters, NServiceBus can't store its data without causing replication conflicts. On startup, NServiceBus will retrieve the cluster topology and throw an exception if the cluster contains more than one Member node:

> RavenDB Persistence does not support RavenDB clusters with more than one Leader/Member node. Only clusters with a single Leader and (optionally) Watcher nodes are supported.

The cluster can support multiple Watcher nodes.

See RavenDB's [Cluster Nodes Types](https://ravendb.net/docs/article-page/4.2/csharp/studio/server/cluster/cluster-view#cluster-nodes-types) documentation for more information.


## Storage format for sagas

RavenDB 4 only allows a string to be used as a document id. However, saga data (inheriting `ContainSagaData` or implementing `IContainSagaData`) has an `Id` property of type `Guid` which is not compatible. To account for this, sagas stored by NServiceBus.RavenDB version 6 and above will be wrapped by a `SagaDataContainer`:

```cs
class SagaDataContainer
{
    public string Id { get; set; }
    public string IdentityDocId { get; set; }
    public IContainSagaData Data { get; set; }
}
```

Sagas stored in the old "unwrapped" format will be automatically converted to the new format the first time they are updated.


## Custom saga finders no longer supported

Because of the change in saga data storage format and the need to convert previously stored sagas on demand, [saga finders](/nservicebus/sagas/saga-finding.md) are no longer supported in RavenDB Persistence. 


## Connect only with a `DocumentStore`

Due to the changes in how RavenDB accepts connection information in RavenDB 4.0, NServiceBus now requires connection information to be provided in the form of a `DocumentStore` in code.

These methods of supplying the connection information are now deprecated and will throw a `NotImplementedException` if used:

* `ConnectionParameters`
* Connection strings
* Default document store at `http://localhost:8080`

See [Connection Options](/persistence/ravendb/connection.md?version=raven_6) for supported connection options in NServiceBus.RavenDB version 6.


## Subscription versioning options removed

The following methods are deprecated and will throw a `NotImplementedException` if used:

* `persistence.DisableSubscriptionVersioning()`
* `persistence.UseLegacyVersionedSubscriptions()`

Subscription versioning now does not include the message assembly version _by default_. Systems using the `DisableSubscriptionVersioning()` method (the new default) can safely remove this call as it is no longer needed.

See the [subscription versioning for NServiceBus.RavenDB version 5](/persistence/ravendb/subscription-versioning?version=raven_4) for more details.


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