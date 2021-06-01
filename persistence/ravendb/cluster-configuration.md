---
title: Cluster configurations with multiple nodes not supported
component: raven
reviewed: 2021-04-12
versions: '[6.0,)'
---

RavenDB version 4 and higher supports configurations with multiple nodes. Support for configurations with a single leader and multiple watchers was originally introduced under the assumption that it’s safe to run without cluster-wide transactions. However, it was discovered that all databases on RavenDB nodes within a cluster behave as primary nodes. That means that any database in a cluster can accept writes. Committing any data safely requires that the data be successfully persisted across a majority of the nodes within the cluster. Optimistic concurrency control cannot be used in cluster scenarios and the only way to enforce this is by using cluster-wide transactions.

Using cluster-wide transactions correctly is complex. Cluster-wide transactions make use of compare-exchange values in order to guarantee consistent writes across the majority of nodes in the cluster. With the current RavenDB api, it’s up to the user to properly implement that across the business data and the NServiceBus data.

DANGER: Not using cluster-wide transactions in the correct form for the business data could result in data loss that is impossible to detect. Due to the potential for data loss, support for RavenDB clusters was withdrawn.

## How to move forward

- If the RavenDB server is configured in a cluster, move back to using a single node. This may not be ideal, as it introduces a single point of failure in the system.
- Move to another persistence. This would require migrating data away from RavenDB to [another persistence](/persistence).

All customers currently using cluster-wide transactions are encouraged to [contact support](https://particular.net/support) to discuss options for transitioning into a safe configuration that causes no data loss.

## ServiceControl

ServiceControl uses an embedded RavenDB database and is not affected.
