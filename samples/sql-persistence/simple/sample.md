---
title: Simple SQL Persistence Usage
summary: Using SQL Persistence to store sagas and timeouts.
reviewed: 2024-12-19
component: SqlPersistence
related:
 - nservicebus/sagas
---

This sample shows a client/server scenario.

> [!WARNING]
> By default all endpoints are started when the solution is run, which means that the sample requires all databases (i.e. SQL Server, MySQL, Oracle, PostreSQL) to be configured to run correctly. In order to run the sample with just one database, disable the relevant endpoints.

include: sqlpersistence-prereqs


## Projects


#### SharedMessages

The shared message contracts used by all endpoints.


### ServerShared

Contains the `OrderSaga` functionality and is referenced by the Server endpoints


### Client

* Sends the `StartOrder` message to either `EndpointMySql` or `EndpointSqlServer`.
* Receives and handles the `OrderCompleted` event.


### Server projects

* `EndpointMySql`, `EndpointSqlServer`, and `EndpointOracle` projects act as "servers" to run the saga instance.
* Receive the `StartOrder` message and initiate an `OrderSaga`.
* `OrderSaga` requests a timeout with an instance of `CompleteOrder` with the saga data.
* `OrderSaga` publishes an `OrderCompleted` event when the `CompleteOrder` timeout fires.


## SQL scripts

Note that only `ServerShared` has the [NServiceBus.Persistence.Sql NuGet package](https://www.nuget.org/packages/NServiceBus.Persistence.Sql) directly referenced. This will cause the script directory `ServerShared\bin\Debug\[TFM]\NServiceBus.Persistence.Sql\[Variant]` to be populated at build time.

These scripts will be copied to the output of each endpoint and executed at startup.

The endpoints know which scripts to execute via the `persistence.SqlVariant();` API at configuration time.

snippet: SqlPersistenceSettings


### Persistence config

Configure the endpoint to use SQL Persistence.


#### MS SQL Server

snippet: sqlServerConfig


#### MySQL

snippet: MySqlConfig


#### Oracle

snippet: OracleConfig


#### PostgreSql

snippet: postgreSqlConfig

## Order saga data

snippet: sagadata

## Order saga

snippet: thesaga

## Querying the saga data

SQL persistence uses the [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/) package to serialize saga data and metadata.

The saga data can be queried using the [JSON querying capabilities of SQL Server](https://docs.microsoft.com/en-us/sql/relational-databases/json/json-data-sql-server).
It is stored inside the `Data` column and can be queried as shown here:

snippet: SqlServerSagaJsonQuery

