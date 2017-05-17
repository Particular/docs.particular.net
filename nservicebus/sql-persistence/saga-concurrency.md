---
title: SqlPersistence Persistence Saga concurrency
versions: '[1,]'
reviewed: 2017-05-17
tags:
 - Persistence
 - Saga
related:
 - nservicebus/sagas/concurrency
---

SqlPersistence honors the required [concurrency semantics](/nservicebus/sagas/concurrency.md) in the following way.

## Concurrent access to non-existing saga instances

The persister makes sure that unique key constraints are created in the data base that will throw should two saga instances with the same correlation value be created at the same time.

## Concurrent access to existing saga instances

The persister provides [optimistic concurrency](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) using a incrementing counter used to provide.

Note: This means that the relevant handle method on the saga will be invoked even though the message might be later rolled back so make sure to not perform any work in saga handlers that can't roll back together with the message. This also means that should there be high levels of concurrency there will be N-1 rollbacks where N is the number of concurrent messages. This can cause throughput issues and might require design changes.