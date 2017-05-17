---
title: RavenDB Persistence Saga concurrency
component: RavenDB
versions: '[6,]'
reviewed: 2017-05-17
tags:
 - Persistence
 - Saga
---

RavenDB honors the required [concurrency semantics](/nservicebus/sagas/concurrency.md) in the following way.

## Concurrent access to non-existing saga instances

RavenDB lacks support for unique indexes so the persister mimics this by creating a separate document for each saga to serve this purpose. The key of this document is set to the value of the correlation property causing RavenDB to throw a exception should two documents with the same correlation value be created at the same time.

## Concurrent access to existing saga instances

The persister makes use of RavenDB optimistic locking cauising concurrency exceptions to be throw should the same saga instance be updated concurrently.

Note: This means that the relevant handle method on the saga will be invoked even though the message might be later rolled back so make sure to not perform any work in saga handlers that can't roll back together with the message. This also means that should there be high levels of concurrency there will be N-1 rollbacks where N is the number of concurrent messages. This can cause throughput issues and might require design changes.