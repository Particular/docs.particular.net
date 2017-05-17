---
title: SqlPersistence Persistence Saga concurrency
component: SqlPersistence
versions: '[1,]'
reviewed: 2017-05-17
tags:
 - Persistence
 - Saga
---

SqlPersistence honors the required [concurrency semantics](/nservicebus/sagas/concurrency.md) in the following way.

## Concurrent access to non-existing saga instances

The persister makes sure that unique key constraints are created in the data base that will throw should two saga instances with the same correlation value be created at the same time.

## Concurrent access to existing saga instances

The persister makes use of optimistic locking cauising concurrency exceptions to be throw should the same saga instance be updated concurrently.

Note: This means that the relevant handle method on the saga will be invoked even though the message might be later rolled back so make sure to not perform any work in saga handlers that can't roll back together with the message. This also means that should there be high levels of concurrency there will be N-1 rollbacks where N is the number of concurrent messages. This can cause throughput issues and might require design changes.