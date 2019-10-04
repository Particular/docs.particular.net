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

By default, SQL persistence uses pessimistic locking combined with [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) when accessing saga. See below for examples of the exceptions thrown when conflicts occur. More information about these scenarios is available in _[saga concurrency](/nservicebus/sagas/concurrency.md)_, including guidance on how to reduce the number of conflicts.

### Creating saga data

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

From Version 4.1.1 onwards, no exceptions will be thrown. Conflicts cannot occur because the persistence uses pessimistic locking.

partial: implementation


