---
title: SQL Persistence saga concurrency
component: SqlPersistence
reviewed: 2018-05-28
related:
 - nservicebus/sagas/concurrency
redirects:
 - nservicebus/sql-persistence/saga-concurrency
---

## Default behavior

When simultaneously handling messages, conflicts may occur. See below for examples of the exceptions which are thrown. _[Saga concurrency](/nservicebus/sagas/concurrency.md)_ explains how these conflicts are handled, and contains guidance for high-load scenarios.

### Starting a saga

Example exception:

```
System.Exception: Failed to ExecuteNonQuery. CommandText:

insert into [dbo].[Samples_SimpleSaga_OrderSaga]
(
    Id,
    Metadata,
    Data,
    PersistenceVersion,
    SagaTypeVersion,
    Concurrency,
    Correlation_OrderId
)
values
(
    @Id,
    @Metadata,
    @Data,
    @PersistenceVersion,
    @SagaTypeVersion,
    1,
    @CorrelationId
) ---> System.Data.SqlClient.SqlException: Cannot insert duplicate key row in object 'dbo.Samples_SimpleSaga_OrderSaga' with unique index 'Index_Correlation_OrderId'. The duplicate key value is (7402c6a7-00bc-40a4-bde2-6e32ec13a7c2).
```

### Updating or deleting saga data

Up to version 4.1, SQL persistence uses [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) when updating or deleting saga data, using an incrementing counter.

Example exception:

```
exception details
```

include: saga-concurrency

partial: updating-deleting


