---
title: SQL Persistence saga concurrency
reviewed: 2018-05-28
related:
 - nservicebus/sagas/concurrency
redirects:
 - nservicebus/sql-persistence/saga-concurrency
---

SQL persistence honors [concurrency semantics](/nservicebus/sagas/concurrency.md) in the following ways.


## Concurrent access to non-existing saga instances

The persister uses unique key constraints that will result in an exception being thrown if two saga instances with the same correlation value are created at the same time.


## Concurrent access to existing saga instances



To ensure correctness when creating new sagas, the persister applies [optimistic concurrency](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) using an incrementing counter. The persister uses an explicit version column combined with the `ReadComitted` isolation level.

include: saga-concurrency

partial: pessimistic

