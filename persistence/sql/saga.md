---
title: Saga Persister
component: SqlPersistence
related:
 - samples/sql-persistence/simple
 - samples/sql-persistence/transitioning-correlation-ids
 - samples/saga/sql-sagafinder
 - samples/saga/migration
 - persistence/sql/saga-concurrency
 - persistence/sql/sqlsaga
redirects:
 - nservicebus/sql-persistence/saga
reviewed: 2016-11-29
---

SQL Persistence supports sagas using the Core [NServiceBus.Saga](/nservicebus/sagas/) API or an [experimental API unique to SQL Persistence](sqlsaga.md) that provides a simpler mapping API.

partial: sqlsaga-required-in-some-versions


## Table Structure


### Table Name

The name used for a saga table consist of two parts.

 * The prefix of the table name is the [Table Prefix](/persistence/sql/install.md#table-prefix) defined at the endpoint level.
 * The suffix of the table name is **either** the saga [Type.Name](https://msdn.microsoft.com/en-us/library/system.type.name.aspx) **or**, if defined, the Table Suffix defined at the saga level.

partial: tablesuffix-snippets

NOTE: Using [Delimited Identifiers](https://technet.microsoft.com/en-us/library/ms176027.aspx) in the TableSuffix is currently **not** supported.


### Columns


#### Id 

The value of `IContainSagaData.Id`. Primary Key.


#### Metadata

Json serialized dictionary containing all NServiceBus managed information about the Saga.


#### Data

Json serialized saga data


#### PersistenceVersion

The Assembly version of the SQL Persister.


#### SagaTypeVersion

The Assembly version of the Assembly where the Saga exists.


#### Correlation Ids

There is between 0 and 2 correlation id columns named `Correlation_[PROPERTYNAME]`. The type will correspond to the .NET type of the mapped property on the saga data.

For each Correlation Id there will be a corresponding index named `Index_Correlation_[PROPERTYNAME]`.


## Correlation Ids

[Saga Message Correlation](/nservicebus/sagas/message-correlation.md) is implemented by promoting the correlation property to the level of a column on the saga table. So when a saga data is persisted the correlation property is copied from the instance and duplicated in a column named by convention (`Correlation_[PROPERTYNAME]`) on the table.

partial: correlation-property


### Correlation Types

Each correlation property type has an equivalent SQL data type.

include: correlationpropertytypes

The following .NET types are interpreted as `CorrelationPropertyType.Int`:

 * [Int16](https://msdn.microsoft.com/en-us/library/system.int16.aspx)
 * [Int32](https://msdn.microsoft.com/en-us/library/system.int32.aspx)
 * [Int64](https://msdn.microsoft.com/en-us/library/system.int64.aspx)
 * [UInt16](https://msdn.microsoft.com/en-us/library/system.uint16.aspx)
 * [UInt32](https://msdn.microsoft.com/en-us/library/system.uint32.aspx)
 * [UInt64](https://msdn.microsoft.com/en-us/library/system.uint64.aspx)


## Json.net Settings


### Custom Settings

Customizes the instance of [JsonSerializerSettings](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonSerializerSettings.htm) used for serialization.

snippet: SqlPersistenceCustomSettings


#### Version / Type specific deserialization settings

The Type and Saga Assembly version are persisted. It is possible to explicitly control the deserialization of sagas based on Version and/or Type. This allows the serialization approach to be evolved forward while avoiding migrations.

snippet: SqlPersistenceJsonSettingsForVersion


### Custom Reader

Customize the creation of the [JsonReader](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonReader.htm).

snippet: SqlPersistenceCustomReader


### Custom Writer

Customize the creation of the [JsonWriter](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonWriter.htm).

snippet: SqlPersistenceCustomWriter
