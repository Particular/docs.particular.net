---
title: Simple Sql Persistence Usage
summary: Using Sql Persistence to store Sagas and Timeouts.
reviewed: 2016-03-21
component: SqlPersistence
tags:
 - Saga
 - Timeout
related:
 - nservicebus/sagas
reviewed: 2016-10-05
---


## Prerequisites


### MS SQL Server

 1. Ensure an instance of SQL Server Express is installed and accessible as `.\SQLEXPRESS`. Create a database `SqlPersistenceSample`. Or, alternatively, change the connection string in the EndpointSqlServer project to point to different Sql Server instance


### MySql

 1. Ensure an instance of MySql is installed and accessible as on `localhost` and port `3306`.
 1. Add the username to access the instance to an environment variable named `MySqlUserName`
 1. Add the password to access the instance to an environment variable named `MySqlPassword`

Or, alternatively, change the connection string in the EndpointMySql project to point to different MySql instance


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
 
 * `EndpointMySql` and `EndpointSqlServer` projects act as "servers" to run the saga instance.
 * Receive the `StartOrder` message and initiate a `OrderSaga`.
 * `OrderSaga` requests a timeout with a `CompleteOrder` data.
 * When the `CompleteOrder` timeout fires the `OrderSaga` publishes a `OrderCompleted` event.


### Sql Scripts

Note that only `ServerShared` has the [NServiceBus.Persistence.Sql.MsBuild NuGet package](https://www.nuget.org/packages/NServiceBus.Persistence.Sql.MsBuild) installed. This will cause the following script directories to be populated at build time 

 * `ServerShared\bin\Debug\NServiceBus.Persistence.Sql\MsSqlServer`
 * `ServerShared\bin\Debug\NServiceBus.Persistence.Sql\MySql`

These scripts will then be copied to the output of both `EndpointMySql` and `EndpointSqlServer` and executed at startup. 

The endpoints know which scripts to execute via the use of the `persistence.SqlVarient();` API usage at configuration time.


### Persistence Config

Configure the endpoint to use Sql Persistence.


#### MS Sql Server

snippet:sqlServerConfig


### MySql

snippet:MySqlConfig


### Order Saga Data

snippet:sagadata


### Order Saga

snippet:thesaga
