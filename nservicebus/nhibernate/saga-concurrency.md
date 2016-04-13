---
title: NHibernate Persistence Saga concurrency settings
summary: How configure concurrency in NHibernate saga persistence
tags:
 - NHibernate
 - Persistence
 - Saga
---

One of the most critical things about persistence of sagas is proper concurrency control. Sagas guarantee business data consistency across long running processes using compensation actions. A failure in concurrency management that leads to creation of an extra instance of a saga instead of routing a message to an existing instance could lead to business data corruption.


## Default behavior

As stated in [saga concurrency](/nservicebus/sagas/concurrency.md), the saga persistence system depends on the data access providing an optimistic approach to concurrency. With NHibernate this results in appending a `WHERE` clause containing all known values of saga data fields when doing `UPDATE`s. This ensures that the saga data is still in the same state as when it was read.

This approach has a downside of very poor performance in high-contention scenarios where a single saga is accessed by multiple message-processing threads. In order to overcome this starting from Version 4.1.0 the NHibernate saga persister uses **additional** pessimistic concurrency control using `UPDLOCK` hint. A lock is created when fetching the saga instance from the database and the lock is held till the end of the transaction preventing other threads from fetching that particular saga. The advantage of this approach that it minimizes the number of retries that would be caused should only the optimistic concurrency was employed. Multiple saga instances can still be processed in parallel.


## Explicit version

The `RowVersion` attribute can be used to explicitly denote a property that should be used for optimistic concurrency control

snippet:NHibernateConcurrencyRowVersion

That property will be included by NHibernate in the `SELECT` and `UPDATE` SQL statements causing concurrency violation error to be raised in case of concurrent updates.

NOTE: Marking a property with `RowVersion` **does not** disable the pessimistic locking optimization. To switch to pure optimistic concurrency adjust the locking strategy to `Read`.


## Adjusting the locking strategy

The `LockMode` attribute can be used to override the default locking strategy.

snippet:NHibernateConcurrencyLockMode
