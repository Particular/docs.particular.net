---
title: RavenDB Persistence Saga concurrency
reviewed: 2017-08-21
related:
 - nservicebus/sagas/concurrency
redirects:
 - nservicebus/ravendb/saga-concurrency
---

include: dtc-warning

RavenDB honors [concurrency semantics](/nservicebus/sagas/concurrency.md) in the following way.


## Concurrent access to non-existing saga instances

RavenDB lacks support for unique indexes, so the persister mimics this by creating a separate document for each saga to serve this purpose. The key of this document is set to the value of the correlation property, this causes RavenDB to throw a exception should two documents with the same correlation value be created at the same time.


## Concurrent access to existing saga instances

The persister makes use of RavenDB optimistic locking causing concurrency exceptions to be throw should the same saga instance be updated concurrently.

include: saga-concurrency
