---
title: Cluster configurations with multiple nodes not supported
component: raven
reviewed: 2021-04-12
versions: '[6.0,)'
---

DANGER: Running in cluster-configurations without the use of cluster-wide transactions for business data could result in data loss that is impossible to detect. Due to the risk data loss, RavenDB clusters are currently not supported.

RavenDB version 4 and higher supports configurations with multiple nodes. Support for configurations with a single leader and multiple watchers was originally introduced under the assumption that it’s safe to run without cluster-wide transactions. However, it was discovered that all databases on RavenDB nodes within a cluster behave as primary nodes. That means that any database in a cluster can accept writes. Committing any data safely requires that the data be successfully persisted across a majority of the nodes within the cluster. Optimistic concurrency control cannot be used in cluster scenarios and the only way to enforce this is by using cluster-wide transactions.

RavenDB version 4 and higher supports multi-node cluster configurations. Database nodes in a database group, distributed across multiple cluster nodes are all operating in master mode, that means that any database node in the cluster can accept writes. Due to missing optimistic concurrency control for cluster-wide transactions, data loss may happen on concurrent writes to different database nodes. Due to this problem, NServiceBus.RavenDB does not currently support cluster configurations.

Using cluster-wide transactions correctly is complex. Cluster-wide transactions make use of compare-exchange values in order to guarantee consistent writes across the majority of nodes in the cluster. With the current RavenDB api, it’s up to the user to properly implement that on their business data.

## How to move forward

- If the database is configured as a multi-node database group, move back to using a single database node by removing additional nodes.
- Move to another persistence. This would require migrating data away from RavenDB to [another persistence](/persistence).

## ServiceControl

ServiceControl uses an embedded RavenDB database and is not affected.
