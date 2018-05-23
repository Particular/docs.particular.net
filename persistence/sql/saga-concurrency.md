---
title: SQL Persistence Saga concurrency
reviewed: 2017-05-17
related:
 - nservicebus/sagas/concurrency
redirects:
 - nservicebus/sql-persistence/saga-concurrency
---

SQL Persistence honors [concurrency semantics](/nservicebus/sagas/concurrency.md) in the following way.


## Concurrent access to non-existing saga instances

The persister uses unique key constraints that will result in an exception being thrown should two saga instances with the same correlation value be created at the same time.


## Concurrent access to existing saga instances



To ensure correctness the persister uses [optimistic concurrency](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) using an incrementing counter. The persister uses an explicit version column combined with the `ReadComitted` isolation level.

include: saga-concurrency

partial: pessimistic

