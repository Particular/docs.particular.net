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

## Prerequisites


partial: prereqs


## Code walk-through

This sample shows a Client + Server scenario.


### Projects


#### SharedMessages

The shared message contracts used by all endpoints.


#### ServerShared

Contains the `OrderSaga` functionality and is referenced by the Server endpoints


####  Client

 * Sends the `StartOrder` message to either `EndpointMySql` or `EndpointSqlServer`.
 * Receives and handles the `OrderCompleted` event.


#### Servers
 
partial: servers


### SQL Scripts

Note that only `ServerShared` has the [NServiceBus.Persistence.Sql.MsBuild NuGet package](https://www.nuget.org/packages/NServiceBus.Persistence.Sql.MsBuild) installed. This will cause the following script directories to be populated at build time 

partial: sqlscripts

The endpoints know which scripts to execute via the use of the `persistence.SqlVariant();` API usage at configuration time.


partial: promote

snippet: SqlPersistenceSettings


### Persistence Config

Configure the endpoint to use SQL Persistence.


#### MS SQL Server

snippet: sqlServerConfig


### MySQL

snippet: MySqlConfig


partial: oracleconfig

### Order Saga Data

snippet: sagadata


### Order Saga

snippet: thesaga

