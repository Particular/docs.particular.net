---
title: Sql Persistence
component: SqlPersistence
tags:
 - Persistence
related:
 - samples/sql-persistence
reviewed: 2016-11-29
---


The Sql Persistence uses [Json.NET](http://www.newtonsoft.com/json) to serialize data and store in a Sql database.


## Supported Sql Implementations

 * [SQL Server](https://www.microsoft.com/en-au/sql-server/) 2012 and up
 * [PostgreSQL](https://www.postgresql.org/) support is planned.


## Usage


snippet:SqlPersistenceUsage


## Json.net Settings


### Custom Settings

Customizes the instance of [JsonSerializerSettings](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonSerializerSettings.htm) used for serialization.

snippet: SqlPersistenceCustomSettings


### Custom Reader

Customize the creation of the [JsonReader](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonReader.htm).

snippet:SqlPersistenceCustomReader


### Custom Writer

Customize the creation of the [JsonWriter](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonWriter.htm).

snippet:SqlPersistenceCustomWriter


## Saga Definition

Sagas need to be decorated with a `[SqlSagaAttribute]`

snippet: SqlPersistenceSaga


### SqlSaga

`SqlSaga<T>` is an extension of `Saga<T>` that has a less verbose mapping API. The `ToSaga` part is inferred from the `[SqlSagaAttribute]`.

snippet: SqlPersistenceSqlSaga