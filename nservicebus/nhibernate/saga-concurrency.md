---
title: NHibernate saga persistence concurrency settings
summary: How configure concurrency in NHibernate saga persistence
tags:
 - NHibernate
 - Persistence
 - Saga
---

One of the most critical things about persistence of sagas is proper concurrency control. Sagas are meant to guarantee business data consistency across long running processes using compensation actions. A failure in concurrency management that leads to creation of an extra instance of a saga instead of routing a message to an existing instance could lead to business data corruption.

## Default behaviour

Starting from version 4.1.0 the default behaviour is pessimistic concurrency using `UPDLOCK` hint. A lock is created when fetching the saga instance from the database and the lock is held till the end of the transaction preventing other threads from fetching that particular saga. The advantage of this approach that it minimizes the number of retries that would be caused should the optimistic concurrency was chosen. Multiple saga instances can still be processed in parallel.

## Enabling optimistic concurrency

The `RowVersion` attribute can be used to denote a property that should be used for optimistic concurrency control

<!-- import NHibernateConcurrencyRowVersion -->

That property will be included by NHibernate in the `SELECT` and `UPDATE` SQL statements causing concurrency violation error to be raised in case of concurrent updates. 

NOTE: Marking a property with `RowVersion` **does not** change the default behaviour of pessimistic locking. If you intend to switch to optimistic concurrency only you need to adjust the locking strategy to `Read`. 

## Adjusting the locking strategy

The `LockMode` attribute can be used to override the default locking strategy. 

<!-- import NHibernateConcurrencyLockMode -->
