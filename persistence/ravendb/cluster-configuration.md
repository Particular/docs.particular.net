---
title: Cluster configurations with multiple nodes not supported
component: raven
reviewed: 2021-04-12
versions: '[6.0,)'
---

RavenDB 4 and up supports configurations with multiple nodes. Previously, support for configurations with a single leader and multiple watchers was introduced, under the assumption that it’s safe to run without the usage of cluster-wide transactions. However, it was discovered that all the databases on RavenDB nodes within a cluster behave as primary nodes. That means that any database in a cluster can accept writes. Committing any data safely requires that the data be successfully persisted across a majority of the nodes within the cluster. The only way to enforce this is by using cluster-wide transactions.

Using cluster-wide transactions correctly is complex. Cluster-wide transactions make use of compare-exchange values in order to guarantee consistent writes across the majority of nodes in the cluster. Each document requires a corresponding compare-exchange value to accomplish that. There’s no mechanism to keep documents and related compare-exchange values in sync, it’s up to the user to manage that. The main challenge is dealing with consistent compare-exchange values when using them to write data.

To be explicit, this is what is required to use cluster-wide transactions correctly:
- Modify all existing documents to store a synchronization identifier. This can be part of the document, or can be stored inside the document’s metadata.
- Create a corresponding compare exchange value for every document. The value of the compare exchange value needs to be set to the synchronization identifier’s value.
- When updating any document, the code must:
- Load the document (and its meta data if needed)
- Load the corresponding compare exchange value
- Compare the document’s synchronization identifier with the one stored in the compare exchange value that corresponds to the document. If they are not the same, it means that the document has already been changed and both documents need to be reloaded.
- Update the document as well as the synchronization identifier
- Update the compare exchange value’s document value with the new synchronization identifier

Given the complexity involved in implementing cluster-wide transactions properly across the business data and the NServiceBus data, Particular was forced to stop supporting clusters in RavenDB for now. NServiceBus.RavenDB can use cluster-wide transactions and compare-exchange values to correctly and safely store NServiceBus data, but cannot make sure any business data modified in an NServiceBus handler is safe. Each handler would need to be reassessed using the steps listed above to ensure correct usage of cluster-wide transactions.

Not using cluster-wide transactions in the correct form for the business data could result in data loss that is impossible to detect. Due to the potential for data loss, support for RavenDB clusters was withdrawn. Unless cluster-wide transactions were already used in the correct form, introducing them comes with a very severe impact on the system.

## How to move forward

- If the RavenDB server is configured in a cluster, move back to using a single node. This is not ideal, as it introduces a single point of failure in the system.
- Move to another persistence option. This would require migrating data from RavenDB to [something else](/persistence).

Don't hesitate to [contact support](https://particular.net/support) if faced with this scenario.

## What about ServiceControl

ServiceControl uses an embedded RavenDB database that is not affected.
