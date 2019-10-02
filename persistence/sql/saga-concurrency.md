---
title: SQL Persistence saga concurrency
component: SqlPersistence
reviewed: 2018-05-28
related:
 - nservicebus/sagas/concurrency
redirects:
 - nservicebus/sql-persistence/saga-concurrency
---



## Default concurrency behavior

The SQL persister applies pessimistic locking combined with [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) when saga instance are updated concurrently.

Please read the guidance on [saga concurrency](/nservicebus/sagas/concurrency.md) on potential improvements.


### Concurrent access to non-existing saga instances

The persister uses unique key constraints that will result in an exception being thrown if two saga instances with the same correlation value are created at the same time.

When this happens the following error is logged:
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

### Concurrent access to existing saga instances

Not possible to get concurrency errors due to pessimistic locking since Version 4.1.1

partial: implementation


