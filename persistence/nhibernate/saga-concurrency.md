---
title: NHibernate Persistence Saga concurrency
component: NHibernate
versions: '[6,]'
reviewed: 2016-10-19
tags:
 - Persistence
 - Saga
related:
 - nservicebus/sagas/concurrency
redirects:
 - nservicebus/nhibernate/saga-concurrency
---

One of the most critical things about persistence of sagas is proper concurrency control. Sagas guarantee business data consistency across long running processes using compensation actions. A failure in concurrency management that leads to creation of an extra instance of a saga instead of routing a message to an existing instance could lead to business data corruption.


## Default behavior

As stated in [saga concurrency](/nservicebus/sagas/concurrency.md), the saga persistence system depends on the data access providing an optimistic approach to concurrency. With NHibernate this results in appending a `WHERE` clause containing all known values of saga data fields when doing `UPDATE`s. This ensures that the saga data is still in the same state as when it was read.

This approach has a downside of poor performance in high-contention scenarios where a single saga is accessed by multiple message-processing threads. These threads read the same saga state and process their messages but only one can succeed persisting the new state. Other threads processing the same message will experience concurrency violation error and need to retry. 

In order to overcome the problem of frequent retries the NHibernate saga persister uses **additional** pessimistic concurrency control using `UPDLOCK` hint. A lock is created when fetching the saga instance from the database and is held till the end of the transaction blocking other threads that try fetching that particular saga. Different saga instances can still be processed concurrently.


## Explicit version

The `RowVersion` attribute can be used to explicitly denote a property that should be used for optimistic concurrency control

snippet: NHibernateConcurrencyRowVersion

That property will be included by NHibernate in the `SELECT` and `UPDATE` SQL statements causing concurrency violation error to be raised in case of concurrent updates.

NOTE: Marking a property with `RowVersion` **does not** disable the pessimistic locking optimization. All it does is replacing the default optimistic concurrency validation that depends on values of all columns with one that is based on that single explicit version column. To switch to pure optimistic concurrency adjust the locking strategy to `Read`.

NOTE: `RowVersion` attribute is not supported when used on derived classes. To specify a custom row version property don't inherit saga data from the `ContainSagaData` class, instead directly implement the `IContainSagaData` interface.

In most cases where the saga data table is only ever accessed by the saga persister it is advisable to use an explicit version because the `UPDATE` SQL statement is much simpler and faster. The downside is that it does not detect concurrency violations if the data is updated by some external party that does not conform to the protocol i.e. does not bump the version field when doing updates. If such an external modification is possible, e.g. when different business process touches the same set of data, it is better to use the default optimistic concurrency validation strategy.

## Adjusting the locking strategy

The `LockMode` attribute can be used to override the default locking strategy.

snippet: NHibernateConcurrencyLockMode


## Customizing the optimistic concurrency handling

In order to customize or switch off optimistic concurrency handling, `optimistic-lock` NHibernate attribute has to be specified in a custom mapping. The [custom mapping sample](/samples/nhibernate/custom-mappings) explains to override the default mapping with a custom one.
