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

To ensure correctness the persister uses [optimistic concurrency](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) using an incrementing counter.

include: saga-concurrency

In addition to that, starting from Version 4.1, the persister also uses pessimistic concurrency to avoid excessive optimistic concurrency conflict for highly congested sagas. The pessimistic concurrency is implemented using `SELECT ... FOR UPDATE` or equivalent construct.
