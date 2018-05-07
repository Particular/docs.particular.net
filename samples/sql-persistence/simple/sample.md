---
title: Simple SQL Persistence Usage
summary: Using SQL Persistence to store Sagas and Timeouts.
reviewed: 2016-10-05
component: SqlPersistence
tags:
 - Saga
 - Timeout
related:
 - nservicebus/sagas
---

This sample shows a Client + Server scenario.

WARNING: By default all endpoints are started when the solution is run, which means that the sample requires all databases (i.e. SQL Server, MySQL, Oracle, PostreSQL) to be configured to run correctly. In order to run the sample with just one database, disable unnecessary endpoints.

include: sqlpersistence-prereqs


## Projects


#### SharedMessages

The shared message contracts used by all endpoints.


### ServerShared

Contains the `OrderSaga` functionality and is referenced by the Server endpoints


### Client

 * Sends the `StartOrder` message to either `EndpointMySql` or `EndpointSqlServer`.
 * Receives and handles the `OrderCompleted` event.


### Servers
 
 * `EndpointMySql`, `EndpointSqlServer`, and `EndpointOracle` projects act as "servers" to run the saga instance.
 * Receive the `StartOrder` message and initiate a `OrderSaga`.
 * `OrderSaga` requests a timeout with a `CompleteOrder` data.
 * When the `CompleteOrder` timeout fires the `OrderSaga` publishes a `OrderCompleted` event.


## SQL Scripts

Note that only `ServerShared` has the [NServiceBus.Persistence.Sql.MsBuild NuGet package](https://www.nuget.org/packages/NServiceBus.Persistence.Sql.MsBuild) installed. This will cause the script director `ServerShared\bin\Debug\NServiceBus.Persistence.Sql\[Variant]` to be populated at build time.

These scripts will then be copied to the output of each endpoint and executed at startup.

The endpoints know which scripts to execute via the use of the `persistence.SqlVariant();` API usage at configuration time.


The scripts produced in this sample are promoted to `$(SolutionDir)PromotedSqlScripts`.

snippet: SqlPersistenceSettings


### Persistence Config

Configure the endpoint to use SQL Persistence.


#### MS SQL Server

snippet: sqlServerConfig


#### MySQL

snippet: MySqlConfig


#### Oracle

snippet: OracleConfig


partial: postgresql


## Order Saga Data

snippet: sagadata


## Order Saga

snippet: thesaga

