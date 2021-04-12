---
title: Cluster configurations with multiple nodes not supported
component: raven
reviewed: 2021-04-12
versions: '[6.0,)'
---

RavenDB 4 and up supports configurations with multiple nodes. Previously, support for configurations with a single leader and multiple watchers was introduced, under the assumption that it’s safe to run without the usage of cluster-wide transactions. However, it was discovered that all the databases on RavenDB nodes within a cluster behave as primary nodes. That means that any database in a cluster can accept writes. Committing any data means that the data must be successfully persisted across a majority of the nodes within the cluster. The way to enforce this is by using cluster-wide transactions.

Using cluster-wide transactions correctly is complex. Cluster-wide transactions make use of compare-exchange values in order to guarantee consistent writes across the majority of nodes in the cluster. Each document requires a corresponding compare-exchange value to accomplish that. There’s no mechanism to keep documents and related compare-exchange values in sync, it’s up to the user to manage that. The main challenge is dealing with consistent compare-exchange values when using them to write data.

To provide some context on what that means practically speaking, to enable the usage of cluster wide transactions it's required to:
- Modify all existing documents to store a synchronization identifier. This can be part of the document, or can be stored inside the document’s metadata.
- Create a corresponding compare exchange value for every document. The value of the compare exchange value needs to be set to the synchronization identifier’s value.
Once that’s done, in order to update a document it's needed to:
- Load the document (and its meta data if needed)
- Load the corresponding compare exchange value
- Compare the document’s synchronization identifier with the one stored in the compare exchange value that corresponds to the document. If they are not the same, it means that the document has already been changed and both documents need to be reloaded.
- Update the document as well as the synchronization identifier
- Update the compare exchange value’s document value with the new synchronization identifier

Given the complexity involved in implementing cluster wide transactions properly across the business data and the persistence data that’s stored by NServiceBus, it was decided to stop supporting clusters in RavenDB for now. Why? The RavenDB persistence can use cluster wide transactions to correctly and safely store NServiceBus data, but the persistence can’t make sure any business data modified in an NServiceBus handler is safe. Each handler would need to be reassessed using the steps listed above to ensure correct usage of cluster-wide transactions.

Not using cluster-wide transactions in the correct form for the business data would result in possible data loss that is impossible to detect. Given that it may surface as possible message loss, it was decided to withdraw support for clusters. Unless cluster wide transactions were already used in the correct form, introducing them comes with a very severe impact on the system.

## How to move forward

- If the RavenDB server is configured in a cluster, move back to using a single node. This is less than ideal, as it introduces a single point of failure in the system.
- Move to another persistence option. This would require migrating data from RavenDB to [another persistence option](/persistence).

Don't hesitate to [contact support](https://particular.net/support) if faced with this scenario.

## What about ServiceControl

ServiceControl uses an embedded RavenDB database. However, there is no impact for ServiceControl users as it still relies on RavenDB 3.5 which makes use of ESENT.
