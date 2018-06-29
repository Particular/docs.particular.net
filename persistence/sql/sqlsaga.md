---
title: SqlSaga Base Class
component: SqlPersistence
related:
 - persistence/sql/saga
reviewed: 2018-06-08
---

`SqlSaga<T>` is an alternate, experimental saga base class for use with SQL Persistence that offers a less verbose mapping API than `NServiceBus.Saga<T>`. It is possible (but not guaranteed) that in the future, the NServiceBus saga class will evolve to look more like `SqlSaga`. Which API to use is largely a matter of personal preference.

partial: required-in-some-versions

## SqlSaga Definition

A saga can be implemented using the `SqlSaga` base class as follows:

snippet: SqlPersistenceSaga

Note that there are some differences to how a standard NServiceBus saga is implemented.

partial: attribute-required


## Correlation Ids

Instead of inferring the correlation property from multiple calls to `.ToSaga(sagaData => sagaData.CorrelationPropertyName)`, the `SqlSaga` base class identifies the correlation property once as a class-level property.


### Single Correlation Id

In most cases there will be a single correlation Id per Saga Type.

snippet: SqlPersistenceSagaWithCorrelation


### Correlation and Transitional Ids

During the migration from one correlation id to another correlation id there may be two correlation is that coexist. See also [Transitioning Correlation ids Sample](/samples/sql-persistence/transitioning-correlation-ids).

snippet: SqlPersistenceSagaWithCorrelationAndTransitional


### No Correlation Id

When implementing a [Custom Saga Finder](/nservicebus/sagas/saga-finding.md) it is possible to have a message that does not map to a   correlation id and instead interrogate the Json serialized data stored in the database.

snippet: SqlPersistenceSagaWithNoMessageMapping


## Table Suffix

A saga's table suffix, which forms part of the [table name](saga.md#table-structure-table-name) in the database, can also be defined more easily using a property override when using the `SqlSaga<T>` base class:

snippet: tableSuffix