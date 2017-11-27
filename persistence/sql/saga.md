---
title: Saga Persister
component: SqlPersistence
related:
 - samples/sql-persistence/simple
 - samples/sql-persistence/transitioning-correlation-ids
 - samples/saga/sql-sagafinder
 - samples/saga/migration
 - persistence/sql/saga-concurrency
redirects:
 - nservicebus/sql-persistence/saga
reviewed: 2016-11-29
---


## Saga Definition

A saga can be implemented as follows:

snippet: SqlPersistenceSaga

Note that there are some differences to how a standard NServiceBus saga is implemented.


### SqlSaga Base Class

All sagas need to inherit from `SqlSaga<T>`. This is a custom base class that has a less verbose mapping API.


partial: attribute-required


## Compile time detection of correlation property

The divergence from the standard standard NServiceBus saga API is required since the SQL Persistence need to be able to determine certain meta data about a saga at compile time.

In a standard Saga, the Correlation Id is configured in the `ConfigureHowToFindSaga` method. On the surface it would seem to be possible to infer the correlation property from that method.

Take this code:

snippet: IlRequirement

At build time the [IL](https://en.wikipedia.org/wiki/Common_Intermediate_Language) of the target assembly is interrogated to generate the SQL installation scripts. The IL will contain enough information to determine that `ToSaga` is called on the property `SagaData.OrderId`. However there are several reasons that an explicit property definition was chosen for defining the Correlation Id over inferring it from the `ConfigureHowToFindSaga`.


### Discovering Sagas

At the IL level it is not possible to discover the base hierarchy of a type given the IL for that type alone. So, in IL, to detect if a given type inherits from `Saga<T>` the full hierarchy of the type needs to be interrogated. This includes loading and interrogating references assemblies, where any types hierarchy extends into those assemblies. This adds significant complexity and performance overheads to the build time operation of generating SQL installation scripts.


### Inferring edge cases

While inferring the Correlation Id from the IL of `ConfigureHowToFindSaga` is possible, there are many edge cases that make this approach problematic. Some of these include:

 * It is possible to [map a message to a complex expression](/nservicebus/sagas/message-correlation.md#message-property-expression). This greatly increases the complexity of accurately determining the Correlation Id due to the higher complexity of the resultant IL.
 * The implementation of `ConfigureHowToFindSaga` means it is evaluated at run time. So it supports branching logic, performing mapping in helper methods, and mapping in various combinations of base classes and child classes. Use of any of these would prevent determining the Correlation Id from the IL.
 * Mapping performed in another assembly. If the mapping is performed in a helper method or base class, and that implementation exists in another assembly, then this would negatively effect build times due to the necessity of loading and parsing the IL for those assemblies.


## Table Structure


### Table Name

The name used for a saga table consist of two parts.

 * The prefix of the table name is the [Table Prefix](/persistence/sql/install.md#table-prefix) defined at the endpoint level.
 * The suffix of the table name is **either** the saga [Type.Name](https://msdn.microsoft.com/en-us/library/system.type.name.aspx) **or**, if defined, the Table Suffix defined at the saga level.

snippet: tableSuffix

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


### No Correlation Id

When implementing a [Custom Saga Finder](/nservicebus/sagas/saga-finding.md) it is possible to have a message that does not map to a   correlation id and instead interrogate the Json serialized data stored in the database.

snippet: SqlPersistenceSagaWithNoMessageMapping


### Single Correlation Id

In most cases there will be a single correlation Id per Saga Type.

snippet: SqlPersistenceSagaWithCorrelation


### Correlation and Transitional Ids

During the migration from one correlation id to another correlation id there may be two correlation is that coexist. See also [Transitioning Correlation ids Sample](/samples/sql-persistence/transitioning-correlation-ids).

snippet: SqlPersistenceSagaWithCorrelationAndTransitional


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
