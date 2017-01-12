---
title: Saga Persister
component: SqlPersistence
tags:
 - Persistence
related:
 - samples/sql-persistence
reviewed: 2016-11-29
---


## Json.net Settings


### Custom Settings

Customizes the instance of [JsonSerializerSettings](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonSerializerSettings.htm) used for serialization.

snippet: SqlPersistenceCustomSettings


#### Version / Type specific deserialization settings

The Type and Saga Assembly version are persisted. It is possible to explicitly control the deserialization of sagas based on Version and/or Type. This allows the serialization approach to be evolved forward while avoiding migrations.

snippet: SqlPersistenceJsonSettingsForVersion


### Custom Reader

Customize the creation of the [JsonReader](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonReader.htm).

snippet:SqlPersistenceCustomReader


### Custom Writer

Customize the creation of the [JsonWriter](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonWriter.htm).

snippet:SqlPersistenceCustomWriter


## Saga Definition

Sagas need to be decorated with a `[SqlSagaAttribute]`. If no [Saga Finder](/nservicebus/sagas/saga-finding.md) is defined then the `correlationProperty` needs to match the [Correlated Saga Property](/nservicebus/sagas/message-correlation.md).

snippet: SqlPersistenceSaga


### SqlSaga

`SqlSaga<T>` is an extension of `Saga<T>` that has a less verbose mapping API. The `ToSaga` part is inferred from the `[SqlSagaAttribute]`.

snippet: SqlPersistenceSqlSaga


## Table Structure


### Columns


#### Id 

The value of `IContainSagaData.Id`. Primary Key.


#### Metadata

Json serialized dictionary containing all NServiceBus managed information about the Saga.


#### Data

Json serialized saga data


#### PersistenceVersion

The Assembly version of the SQL Persister


#### SagaTypeVersion

The Assembly version of the Assembly where the Saga exists.


#### Concurrency

Incrementing counter used to provide [optimistic concurrency](https://en.wikipedia.org/wiki/Optimistic_concurrency_control).


#### Correlation Ids

There is between 0 and 2 correlation id columns named `Correlation_[PROPERTYNAME]`. The type will correspond to the .net type of .net type of the mapped property on the saga data. See [Correlation Ids](#correlation-ids).

For each Correlation Id there will be a corresponding index named `Index_Correlation_[PROPERTYNAME]`.


## Correlation Ids

[Saga Message Correlation](/nservicebus/sagas/message-correlation.md) is implemented by promoting the correlation property to the level of a column on the saga table. So when a saga data is persisted the correlation property is copied from the instance and duplicated in a column named by convention (`Correlation_[PROPERTYNAME]`) on the table.


### No Correlation Id

When implementing a [Custom Saga Finder](/nservicebus/sagas/saga-finding.md) it is possible to have no correlation id and instead interrogate the Json serialized data stored in the database.

snippet: SqlPersistenceSagaWithNoCorrelation


### Single Correlation Id

In most cases there will be a single correlation Id per Saga Type.

snippet: SqlPersistenceSagaWithCorrelation


### Correlation and Transitional Ids

During the migration from one correlation id to another correlation id there may be two correlation is that coexist. See also [Transitioning Correlation ids Sample](/samples/sql-persistence/transitioning-correlation-ids).

snippet: SqlPersistenceSagaWithCorrelationAndTransitional