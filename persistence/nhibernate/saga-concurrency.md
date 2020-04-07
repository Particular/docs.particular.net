---
title: NHibernate Persistence Saga Concurrency
summary: How to control concurrency in sagas with the NHibernate persistence
component: NHibernate
versions: '[6,]'
reviewed: 2020-04-07
tags:
 - Persistence
 - Saga
related:
 - nservicebus/sagas/concurrency
redirects:
 - nservicebus/nhibernate/saga-concurrency
---

One of the most critical things about persistence of sagas is proper concurrency control. Sagas guarantee business data consistency across long running processes using compensating actions. A failure in concurrency management that leads to creation of an extra instance of a saga instead of routing a message to an existing instance could lead to business data corruption.


## Default behavior

When simultaneously handling messages, conflicts may occur. See below for examples of the exceptions which are thrown. _[Saga concurrency](/nservicebus/sagas/concurrency.md)_ explains how these conflicts are handled, and contains guidance for high-load scenarios.

### Starting a saga

Example exception:

```
NHibernate.Exceptions.GenericADOException: could not execute batch command.[SQL: SQL not available] ---> System.Data.SqlClient.SqlException: Violation of UNIQUE KEY constraint 'UQ__OrderSag__C3905BCE71EF212B'. Cannot insert duplicate key in object 'dbo.OrderSagaData'. The duplicate key value is (e87490ba-bb56-4693-9c0a-cf4f95736e06).
```

### Updating or deleting saga data

No exceptions will be thrown. Conflicts cannot occur because the persistence uses pessimistic locking. Pessimistic locking is achieved by performing a **SELECT ... FOR UPDATE**, see [NHibernate Chapter 12. Transactions And Concurrency](https://nhibernate.info/doc/nhibernate-reference/transactions.html).

## Custom behavior

### Explicit version

The `RowVersion` attribute can be used to explicitly denote a property that should be used for optimistic concurrency control. An update will then compare against this single property instead of comparing it against the previous state of all properties which results in a more efficient comparison.

snippet: NHibernateConcurrencyRowVersion

That property will be included by NHibernate in the `SELECT` and `UPDATE` SQL statements causing a concurrency violation error to be raised in case of concurrent updates.

NOTE: Marking a property with `RowVersion` **does not** disable the pessimistic locking optimization. All it does is replace the default optimistic concurrency validation that depends on values of all columns with one that is based on that single explicit version column. To switch to pure optimistic concurrency adjust the locking strategy to `Read`.

NOTE: The `RowVersion` attribute is not supported when used on derived classes. To specify a custom row version property, don't inherit saga data from the `ContainSagaData` class; instead directly implement the `IContainSagaData` interface.

In most cases where the saga data table is only ever accessed by the saga persister, it is advisable to use an explicit version because the `UPDATE` SQL statement is much simpler and faster. The downside is that it does not detect concurrency violations if the data is updated by some external party that does not conform to the protocol i.e. does not bump the version field when doing updates. If such an external modification is possible, e.g. when different business process touches the same set of data, it is better to use the default optimistic concurrency validation strategy.

### Adjusting the locking strategy

The `LockMode` attribute can be used to override the default locking strategy.

snippet: NHibernateConcurrencyLockMode


### Customizing the optimistic concurrency handling

In order to customize or switch off optimistic concurrency handling, the `optimistic-lock` NHibernate attribute has to be specified in a custom mapping. The [custom mapping sample](/samples/nhibernate/custom-mappings) explains how to override the default mapping with a custom one.
