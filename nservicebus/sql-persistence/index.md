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

 * [SQL Server](https://www.microsoft.com/en-au/sql-server/)
 * [MySql](https://www.mysql.com/)


## Usage


### SQL Server

snippet:SqlPersistenceUsageSqlServer


### MySql

Using the [MySql.Data NuGet Package](https://www.nuget.org/packages/MySql.Data/).

snippet:SqlPersistenceUsageMySql


## SqlStorageSession

The current [DbConnection](https://msdn.microsoft.com/en-us/library/system.data.common.dbconnection.aspx) and [DbTransaction](https://msdn.microsoft.com/en-us/library/system.data.common.dbtransaction.aspx) can be accessed via the current context.


### Using in a Handler

snippet: handler-sqlPersistenceSession


### Using in a Saga

snippet: saga-sqlPersistenceSession
