---
title: Installing RavenDB
summary: How to install RavenDB when using RavenDB persistence for various versions of NServiceBus.
component: core
versions: "[3,)"
reviewed: 2019-06-10
related:
 - nservicebus/operations
redirects:
 - nservicebus/using-ravendb-in-nservicebus-installing
 - nservicebus/ravendb/installation
---

include: dtc-warning

include: cluster-configuration-warning


To install RavenDB, [download the server](https://ravendb.net/download) and install as described in the [RavenDB documentation](https://ravendb.net/docs/) or use a hosted RavenDB provider such as [RavenHQ](https://www.ravenhq.com/).

RavenDB should only be used for NServiceBus persistence when the endpoint's business data is already stored in RavenDB. NServiceBus shares the same `DocumentStore` object used for business data, configured using the [RavenDB connection options](connection.md).


## Upgrading RavenDB

To upgrade an existing RavenDB installation, refer to the [RavenDB upgrade process](https://ravendb.net/docs/search/latest/csharp?searchTerm=server-administration%20upgrade).

It is strongly recommended to backup all databases before upgrading.
