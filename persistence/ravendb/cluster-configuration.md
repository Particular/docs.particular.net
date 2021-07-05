---
title: Cluster configurations with multiple nodes not supported
component: raven
reviewed: 2021-04-12
versions: '[6.0,)'
---

DANGER: Cluster configurations and cluster-wide transactions are currently not supported in the RavenDB persistence.

RavenDB version 4 and higher supports multi-node cluster configurations. The RavenDB persistence will guard against cluster configurations, as they can lead to data loss if not used correctly (with cluster-wide transactions).

Database nodes in a database group, distributed across multiple cluster nodes operate in master mode, which means that any database node in the cluster can accept writes. Due to missing optimistic concurrency control for cluster-wide transactions, data loss may happen on concurrent writes to different database nodes.

partial: config
