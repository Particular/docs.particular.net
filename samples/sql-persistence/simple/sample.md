---
title: Simple SQL Persistence Usage
summary: Using SQL Persistence to store sagas and timeouts.
reviewed: 2020-03-26
component: SqlPersistence
related:
 - nservicebus/sagas
---

This sample shows a client/server scenario.

WARNING: By default all endpoints are started when the solution is run, which means that the sample requires all databases (i.e. SQL Server, MySQL, Oracle, PostreSQL) to be configured to run correctly. In order to run the sample with just one database, disable the relevant endpoints.

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

partial: scripts-package

These scripts will be copied to the output of each endpoint and executed at startup.

The endpoints know which scripts to execute via the `persistence.SqlVariant();` API at configuration time.

The scripts produced in this sample are promoted to `$(SolutionDir)PromotedSqlScripts`.

snippet: SqlPersistenceSettings


### Persistence config

Configure the endpoint to use SQL Persistence.


#### MS SQL Server

snippet: sqlServerConfig


#### MySQL

snippet: MySqlConfig


#### Oracle

snippet: OracleConfig


partial: postgresql


## Order saga data

snippet: sagadata

## Order saga

snippet: thesaga

## Querying the saga data

SQL persistence uses the [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/) package to serialize saga data and metadata.

The saga data can be queried using the [JSON querying capababilities of SQL Server](https://docs.microsoft.com/en-us/sql/relational-databases/json/json-data-sql-server).
It is stored inside the `Data` column and can be queried as shown here:

snippet: SqlServerSagaJsonQuery

