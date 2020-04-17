---
title: SqlSaga base class
summary: An alternate base class for use with SQL persistence
component: SqlPersistence
related:
 - persistence/sql/saga
reviewed: 2020-04-17
---

`SqlSaga<T>` is an alternate saga base class for use with SQL persistence that offers a less verbose mapping API than `NServiceBus.Saga<T>`. It's generally advisable to use `NServiceBus.Saga<T>` by default for most new projects, and to switch to `SqlSaga<T>` when advantageous to cut down on the need for repetitive `.ToSaga(...)` expressions in sagas that handle several message types.

partial: required-in-some-versions

## SqlSaga definition

A saga can be implemented using the `SqlSaga` base class as follows:

snippet: SqlPersistenceSaga

Note that there are differences to how a standard NServiceBus saga is implemented.

partial: attribute-required


## Correlation IDs

Instead of inferring the correlation property from multiple calls to `.ToSaga(sagaData => sagaData.CorrelationPropertyName)`, the `SqlSaga` base class identifies the correlation property once as a class-level property.


### Single Correlation ID

In most cases there will be a single correlation ID per Saga Type.

snippet: SqlPersistenceSagaWithCorrelation


### Correlation and Transitional IDs

During the migration from one correlation ID to another correlation ID, there may be two correlation IDs that co-exist. See also the [Transitioning Correlation IDs Sample](/samples/sql-persistence/transitioning-correlation-ids).

snippet: SqlPersistenceSagaWithCorrelationAndTransitional


### No Correlation ID

When implementing a [custom saga finder](/nservicebus/sagas/saga-finding.md) it is possible to have a message that does not map to a correlation ID and instead interrogate the JSON-serialized data stored in the database.

snippet: SqlPersistenceSagaWithNoMessageMapping


## Table suffix

A saga's table suffix, which forms part of the [table name](saga.md#table-structure-table-name) in the database, can also be defined more easily using a property override when using the `SqlSaga<T>` base class:

snippet: tableSuffix
