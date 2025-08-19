---
title: SQL Persistence saga concurrency
component: SqlPersistence
reviewed: 2025-03-18
related:
 - nservicebus/sagas/concurrency
redirects:
 - nservicebus/sql-persistence/saga-concurrency
---

## Default behavior

When simultaneously handling messages, conflicts may occur. See below for examples. [Saga concurrency](/nservicebus/sagas/concurrency.md) explains how these conflicts are handled and contains guidance for high-load scenarios.

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

Starting in version 4.1.1, conflicts cannot occur because the persistence uses pessimistic locking. Pessimistic locking is achieved by performing a `SELECT ... FOR UPDATE` or its dialect-specific equivalent.

Up to and including version 4.1.0, SQL persistence uses [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) when updating or deleting saga data.

Example exception:

```
System.Exception: Optimistic concurrency violation when trying to complete saga OrderSaga 699d0b1a-e2bf-49fd-8f26-aadf01009eaf. Expected version 4.
```

include: saga-concurrency