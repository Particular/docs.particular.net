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

include: dtc-warning

This update supports upgrading to RavenDB 4.2. The upgrade to RavenDB 4 is a **major** upgrade. It's not backward compatible, and the [API has a lot of breaking changes](https://ravendb.net/docs/article-page/4.2/csharp/migration/client-api/introduction). Additionally, the data storage format has changed, so databases need to have their [data migrated to a new RavenDB 4 database](https://ravendb.net/docs/article-page/4.2/csharp/migration/server/data-migration).


## Supported cluster configurations

RavenDB 4 [does not support Windows Failover Clusters](https://groups.google.com/forum/#!msg/ravendb/_TxAFNlCXik/n1RS_m-SAwAJ) and instead has built-in support for [RavenDB Clustering](https://ravendb.net/docs/article-page/4.2/csharp/server/clustering/overview).

When using RavenDB clustering, only clusters with one full Member node (the Leader) are supported. In multi-member clusters, NServiceBus can't store its data without causing replication conflicts. On startup, NServiceBus will retrieve the cluster topology and throw an exception if the cluster contains more than one Member node:

> RavenDB Persistence does not support RavenDB clusters with more than one Leader/Member node. Only clusters with a single Leader and (optionally) Watcher nodes are supported.

The cluster can support multiple Watcher nodes.

See RavenDB's [Cluster Nodes Types](https://ravendb.net/docs/article-page/4.2/csharp/studio/server/cluster/cluster-view#cluster-nodes-types) documentation for more information.


## Sagas

stub


## ConnectionParams

stub
