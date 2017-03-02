---
title: Saga Persister
component: SqlPersistence
related:
 - samples/sql-persistence/simple
 - samples/sql-persistence/transitioning-correlation-ids
 - samples/saga/sql-sagafinder
 - samples/saga/migration
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


## Requirement for the SqlSagaAttribute

When looking at a standards Saga's `ConfigureHowToFindSaga` it is logical to come to the conclusion that it should be possible to infer the Correlation Id and hence remove the requirement for defining a `[SqlSagaAttribute]`.

Take this code:

snippet: AttributeRequirement

At build time the [IL](https://en.wikipedia.org/wiki/Common_Intermediate_Language) of the target assembly is interrogated to generate the SQL installation scripts. The IL will contain enough information to determine that `ToSaga` is called on the property `SagaData.OrderId`. However there are several reasons that an explicit attribute was chosen for defining the Correlation Id over inferring it from the `ConfigureHowToFindSaga`.


### Discovering Sagas

At the IL level it is not possible to discover the base hierarchy of a type given the IL for that type alone. So, in IL, to detect if a given type inherits from `Saga<T>` the full hierarchy of the type needs to be interrogated. This includes loading and interrogating references assemblies, where any types hierarchy extends into those assemblies. This adds significant complexity and performance overheads to the build time operation of generating SQL installation scripts. The explicit `[SqlSagaAttribute]` allows saga detection via a simpler type scan of the target assembly.


### Inferring edge cases

While inferring the Correlation Id from the IL of `ConfigureHowToFindSaga` is possible there are many edge cases that make this approach problematic. Some of these include:

 * It is possible to [map message to a complex expression](/nservicebus/sagas/message-correlation.md#message-property-expression). This greatly increased the complexity of accurately determining the Correlation Id due to the higher complexity of the resultant IL.
 * The implementation of `ConfigureHowToFindSaga` means it is evaluated at run time. So it supports branching logic, performing mapping in helper methods, and mapping in various combinations of base classes and child classes. Use of any of these would prevent determining the Correlation Id from the IL.
 * Mapping performed in another assembly. If the mapping is performed in a helper method or base class, and that implementation exists in another assembly, then this would negatively effect build times due to the necessity of loading and parsing the IL for those assemblies.


### Attribute required for other purposes

It is likely a consumer of the persister will need to use the `[SqlSagaAttribute]` configurations options other than Correlation Id. For example to control the [table name for a saga](/nservicebus/sql-persistence/saga.md#table-structure-table-name). Given this likelihood, the argument for avoiding the necessity of an attribute is diminished.


## Table Structure


### Table Name

The name used for a saga table consist of two parts.

 * The prefix of the table name is the [Table Prefix](/nservicebus/sql-persistence/#installation-table-prefix) defined at the endpoint level.
 * The suffix of the table name is **either** the saga [Type.Name](https://msdn.microsoft.com/en-us/library/system.type.name.aspx) **or** the Table Suffix defined in the `[SqlSagaAttribute]`.

snippet: tableSuffix


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


#### Concurrency

Incrementing counter used to provide [optimistic concurrency](https://en.wikipedia.org/wiki/Optimistic_concurrency_control).


#### Correlation Ids

There is between 0 and 2 correlation id columns named `Correlation_[PROPERTYNAME]`. The type will correspond to the .net type of .net type of the mapped property on the saga data.

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


### Correlation Types

Each correlation property type has an equivalent sql data type.

include: correlationpropertytypes

The following .NET types are interpreted as `CorrelationPropertyType.Int`:

 * [Int16](https://msdn.microsoft.com/en-us/library/system.int16.aspx)
 * [Int32](https://msdn.microsoft.com/en-us/library/system.int32.aspx)
 * [Int64](https://msdn.microsoft.com/en-us/library/system.int64.aspx)
 * [UInt16](https://msdn.microsoft.com/en-us/library/system.uint16.aspx)
 * [UInt32](https://msdn.microsoft.com/en-us/library/system.uint32.aspx)
 * [UInt64](https://msdn.microsoft.com/en-us/library/system.uint64.aspx)
